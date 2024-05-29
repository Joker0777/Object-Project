using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

public class EnemyBehaviourSystem : UnitSystems
{

    protected IMovable movement;
    private WeaponSystem weapon;

    protected float _attackRange = 3;


    [SerializeField] protected float _detectRange = 7;
    [SerializeField] protected float _stopRange = 1;

    [SerializeField] protected float _playerAttackRange = 5;
   // [SerializeField] protected float _asteroidAttackRange = 2;
  //  [SerializeField] protected float _targetEvadeRange = 2;
    [SerializeField] protected float asteroidAvoidanceRange = 10f;

    [SerializeField]float angleThresholdWeapon1 = 0.9f;
    [SerializeField] float angleThresholdWeapon2 = .9f;

    protected EnemyState _currentState = EnemyState.Patrol;

    [SerializeField]protected GameObject _playerTarget;
  //  protected GameObject _closestAsteroid;
  //  protected GameObject _currentTarget;

    protected float _targetToWeaponForwardDot;
   // protected float angleToTarget;
    protected float _angleToTurn;
    protected Vector2 moveInput;
    protected Vector2 directionToMove;
    protected Vector2 currentDirection;
 //   protected Vector2 directionAwayFromTarget;

    protected override void Awake()
    {
        base.Awake();
        movement = GetComponentInChildren<IMovable>();
        weapon = GetComponentInChildren<WeaponSystem>();

        if (movement == null)
        {
            Debug.Log("Movment not assigned or does not implement IMovable");
        }

        if (weapon == null)
        {
            Debug.Log("Weapon not assigned or does not implement IWeapon");
        }
    }
   


    protected virtual void Update()
    {
        UpdateMovement();

        switch (_currentState)
        {
            case EnemyState.Patrol:
                Patrol();
                break;

            case EnemyState.DetectTarget:
                DetectTarget();
                break;

            case EnemyState.AttackTarget:
                AttackTarget();
                break;   
        }
    }

    private void UpdateMovement()
    {
        currentDirection = Vector2.Lerp(currentDirection, directionToMove, Time.deltaTime * 5f);
        _angleToTurn = Mathf.Atan2(currentDirection.y, currentDirection.x) * Mathf.Rad2Deg;
      //  angleToTarget = Vector2.Angle(transform.up, currentDirection);

        movement?.Move(moveInput, _angleToTurn);
    }

    protected override void Start()
    {
        base.Start();
        weapon?.SwitchWeapon(1);
    }

    protected bool InRange(float range, GameObject target)
    {
        return target != null && Vector2.Distance(target.transform.position, transform.position) < range;
    }

    protected void MoveAwayFromTarget(GameObject target)
    {
  
        if (target == null) return;

        Debug.Log("In move away from target     target is " + target.name);

        Vector2 targetDirection = (target.transform.position - transform.position).normalized;
     

        float relativeDirection = Vector3.Cross(transform.up, targetDirection).z;//if greater than 0 on the right side
        float dotProduct = Vector2.Dot(transform.up, targetDirection);

        float angleToMove = 0f;
        if (dotProduct > 0) // Target is in front or to the front side
        {
            angleToMove = Mathf.Lerp(80, 10, dotProduct);
            angleToMove *= -1; // Move in the opposite direction
        }
        else // Target is behind or to the back side
        {
            angleToMove = Mathf.Lerp(10, 80, -dotProduct);
        }

        angleToMove *= (relativeDirection > 0) ? 1 : -1; // Determine if we move to the left or right based on relative direction
        float angleInRadians = angleToMove * Mathf.Deg2Rad;

        // Calculate the new direction based on the angle
        directionToMove = new Vector2(
            targetDirection.x * Mathf.Cos(angleInRadians) - targetDirection.y * Mathf.Sin(angleInRadians),
            targetDirection.x * Mathf.Sin(angleInRadians) + targetDirection.y * Mathf.Cos(angleInRadians)
        ).normalized;

        moveInput = Vector2.up;
    }
  
    protected void Patrol()
    {
        Debug.Log("In Patrol");
        
        moveInput = Vector2.zero;

        if (InRange(_detectRange, _playerTarget))
        {
            _currentState = EnemyState.DetectTarget;         
        }
    }

    protected void AttackTarget()
    {
        FollowTarget();

        if(!InRange(_attackRange, _playerTarget)) 
        {
            _currentState = EnemyState.DetectTarget;
            weapon?.SwitchWeapon(1);
        }
        else if (_targetToWeaponForwardDot >= angleThresholdWeapon1)
        {
            weapon?.FireWeapon();
        }
    }

    protected void DetectTarget()
    {
        FollowTarget();

        if (weapon?.WeaponCount > 1 && _targetToWeaponForwardDot >= angleThresholdWeapon2)
        {
            weapon?.FireWeapon();
        }


        if (InRange(_attackRange, _playerTarget))
        {
            _currentState = EnemyState.AttackTarget;
            weapon?.SwitchWeapon(0);
        }
        else if (!InRange(_detectRange, _playerTarget))
        {
            _currentState = EnemyState.Patrol;
        }
    }

    protected void FollowTarget()
    {
        if (_playerTarget == null) return;

        List<GameObject> obsticles = GetObsticlesInRange();
        Vector2 playerDirection = (_playerTarget.transform.position - transform.position).normalized;
        Vector2 collisionAvoidenceOffset = GetAvoidenceOffset(obsticles);
        Vector2 combinedDirection = (playerDirection + collisionAvoidenceOffset).normalized;

        directionToMove = combinedDirection;

        moveInput = InRange(_stopRange, _playerTarget) ? Vector2.zero : Vector2.up;

        _targetToWeaponForwardDot = Vector2.Dot(transform.up, directionToMove);

    }

    private List<GameObject> GetObsticlesInRange()
    {
        var colliders = Physics2D.OverlapCircleAll(transform.position, asteroidAvoidanceRange);
        var obsticles = new List<GameObject>();

        foreach (var collider in colliders)
        {
            if (collider.gameObject != gameObject && !collider.CompareTag("Player"))
            {
                obsticles.Add(collider.gameObject);
            }
        }

        return obsticles;
    }

    private Vector2 GetAvoidenceOffset(List<GameObject> Obsticles)
    {
        Vector2 avoidanceOffset = Vector2.zero;

        foreach (var obsticles in Obsticles)
        {
            Vector2 awayFromObsticle = (transform.position - obsticles.transform.position).normalized;
            Vector2 toObsticle = (obsticles.transform.position - transform.position).normalized;

            Vector2 perpendicularLeft = new Vector2(-toObsticle.y, toObsticle.x);
            Vector2 perpendicularRight = new Vector2(toObsticle.y, -toObsticle.x);

            float dotProduct = Vector2.Dot(transform.right, toObsticle);

            Vector2 sideAvoidance;

            if (dotProduct > 0) 
            {
                sideAvoidance = perpendicularLeft; 
            }
            else 
            {
                sideAvoidance = perpendicularRight; 
            }

            avoidanceOffset += (awayFromObsticle + sideAvoidance) / Vector2.Distance(transform.position, obsticles.transform.position);
        }

        return avoidanceOffset.normalized;
          
    }
}
