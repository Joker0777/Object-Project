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
    [SerializeField] protected float _playerAttackRange = 7;
    [SerializeField] protected float obsticleAvoidanceRange = 10f;

    [SerializeField]float angleThresholdWeapon1 = 0.9f;

    protected EnemyState _currentState = EnemyState.Patrol;

    protected GameObject _target;
    [SerializeField] protected LayerMask ignoreObsticles;
    [SerializeField] protected LayerMask _targetLayer;

    protected float _targetToWeaponForwardDot;
    protected float _angleToTurn;
    protected Vector2 moveInput;
    protected Vector2 directionToMove;
    protected Vector2 currentDirection;


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
        if(_target == null)
        {
            FindTarget();
        }
        
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

        movement?.Move(moveInput, _angleToTurn);
    }

    
    protected bool InRangeOfTarget(float range, GameObject target)
    {
        return target != null && Vector2.Distance(target.transform.position, transform.position) < range;
    }

    protected bool InLineWtihTarget(GameObject target, float inLineThreshold)
    {
        Vector2 targetDirection = (target.transform.position - transform.position).normalized;
        return Vector2.Dot(transform.up, targetDirection) >= inLineThreshold;
    }

    protected void FindTarget()
    {      
        var colliders = Physics2D.OverlapCircleAll(transform.position, _detectRange, _targetLayer);
        if(colliders != null && colliders.Length == 1)
        {                
              _target = colliders[0].gameObject;       
        }
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
        moveInput = Vector2.zero;

        if (InRangeOfTarget(_detectRange, _target))
        {
            _currentState = EnemyState.DetectTarget;         
        }
    }

    protected void AttackTarget()
    {
        FollowTarget();

        if(!InRangeOfTarget(_attackRange, _target)) 
        {
            _currentState = EnemyState.DetectTarget;
        }
        else if (InLineWtihTarget(_target, angleThresholdWeapon1))
        {
            weapon?.FireWeapon();
        }
    }

    protected void DetectTarget()
    {
        FollowTarget();

        if (InRangeOfTarget(_attackRange, _target))
        {
            _currentState = EnemyState.AttackTarget;
        }
        else if (!InRangeOfTarget(_detectRange, _target))
        {
            _currentState = EnemyState.Patrol;
        }
    }

    protected void FollowTarget()
    {
        if (_target == null) return;

        List<GameObject> obsticles = GetObsticlesInRange();
        Vector2 playerDirection = (_target.transform.position - transform.position).normalized;
        Vector2 collisionAvoidenceOffset = GetAvoidenceOffset(obsticles);
        Vector2 combinedDirection = (playerDirection + collisionAvoidenceOffset).normalized;

        directionToMove = combinedDirection;

        moveInput = InRangeOfTarget(_stopRange, _target) ? Vector2.zero : Vector2.up;
    }

    private List<GameObject> GetObsticlesInRange()
    {
        var colliders = Physics2D.OverlapCircleAll(transform.position, obsticleAvoidanceRange, ~ignoreObsticles.value);
        var obsticles = new List<GameObject>();

        foreach (var collider in colliders)
        {
            
            if (collider.gameObject != gameObject && collider.gameObject != _target)
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

            float dotProductSide = Vector2.Dot(transform.right, toObsticle);
            float dotProductFront = Vector2.Dot(transform.up, toObsticle);

            Vector2 sideAvoidance = Vector2.zero;

             //if ((dotProductSide > 0 && dotProductFront > 0) || (dotProductSide < 0 && dotProductFront < 0))
             if (dotProductSide > 0 && dotProductFront > 0)
            {
                sideAvoidance = perpendicularLeft; 
            }
            // else if((dotProductSide < 0 && dotProductFront > 0) ||(dotProductSide > 0 && dotProductFront < 0))
             else if(dotProductSide < 0 && dotProductFront > 0)
            {
                sideAvoidance = perpendicularRight; 
            }
            //float influenceFactor = Mathf.Lerp(1f, minInfluenceFactor, Vector2.Distance(transform.position, obsticles.transform.position) / asteroidAvoidanceRange);
            avoidanceOffset += (awayFromObsticle + sideAvoidance)  / Vector2.Distance(transform.position, obsticles.transform.position);
       
        }
        Debug.Log(avoidanceOffset.ToString());

        return avoidanceOffset.normalized;          
    }
}
