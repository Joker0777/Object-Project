using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class EnemyBehaviourSystem : UnitSystems
{

    protected IMovable movement;
    private WeaponSystem weapon;

    protected float _attackRange = 3;


    [SerializeField] protected float _detectRange = 7;
    [SerializeField] protected float _stopRange = 1;

    [SerializeField] protected float _playerAttackRange = 4;
    [SerializeField] protected float _asteroidAttackRange = 2;
    [SerializeField] protected float _targetEvadeRange = 3;

    [SerializeField]float angleThresholdWeapon1 = 0.9f;
    [SerializeField] float angleThresholdWeapon2 = .9f;

    protected EnemyState _currentState = EnemyState.Patrol;

    protected GameObject _playerTarget;
    protected GameObject _closestAsteroid;
    protected GameObject _currentTarget;

    protected float _targetToWeaponForwardDot;
    protected float angleToTarget;
    protected float _angleToTurn;
    protected Vector2 moveInput;
    protected Vector2 directionToMove;
    protected Vector2 currentDirection;
    protected Vector2 directionAwayFromTarget;

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


        _currentTarget = GetTarget();

        switch (_currentState)
        {
            case EnemyState.Patrol:
                Patrol();
                break;

            case EnemyState.DetectTarget:
                DetectTarget();
                break;

            case EnemyState.AttackTarget:
                AttackTarget(_currentTarget);
                break;

              case EnemyState.EvadeTarget:
                EvadeTarget();
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

    protected void FollowTarget(GameObject target)
    {
        if (target == null) return;
        
            moveInput = InRange(_stopRange, _currentTarget) ? Vector2.zero : Vector2.up;
            directionToMove = (target.transform.position - transform.position).normalized;
            _targetToWeaponForwardDot = Vector2.Dot(transform.up, directionToMove);
        
    }

    protected void MoveAwayFromTarget(GameObject target)
    {
  
        if (target == null) return;

        Vector2 targetDirection = (target.transform.position - transform.position).normalized;
        Vector2 perpendicularDirection = new Vector2(-targetDirection.y, targetDirection.x);

        float relativeDirection = Vector3.Cross(transform.up, targetDirection).z;//if greater than 0 on the right side
        float dotProduct = Vector2.Dot(transform.up, targetDirection);

        if (dotProduct > 0.5f)
        {
            directionToMove = perpendicularDirection;
        }
        else
        {
            directionToMove = (Random.value > 0.5f ? 1 : -1) * perpendicularDirection;
        }

        moveInput = Vector2.up;
    }
  
    protected void Patrol()
    {
        moveInput = Vector2.zero;

        if (_currentTarget != null)
        {
            _currentState = EnemyState.DetectTarget;         
        }
    }

    protected void AttackTarget(GameObject target)
    {
        FollowTarget(target);

        Debug.Log("In attack target. Attack target is " + target.name);

        if(!InRange(_attackRange, target)) 
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
        FollowTarget(_currentTarget);

        Debug.Log("In detect target. Current target is " + _currentTarget.name);

        if (weapon?.WeaponCount > 1 && _targetToWeaponForwardDot >= angleThresholdWeapon2)
        {
            weapon?.FireWeapon();
        }


        if (InRange(_attackRange, _currentTarget))
        {
            _currentState = EnemyState.AttackTarget;
            weapon?.SwitchWeapon(0);
        }
        else if (!InRange(_detectRange, _currentTarget))
        {
            _currentState = EnemyState.Patrol;
        }
    }


    protected void EvadeTarget()
    {
        MoveAwayFromTarget(_currentTarget);

      //  if (InRange(_asteroidAttackRange, _currentTarget))
      //  {
       ///     _currentState = EnemyState.AttackTarget;
   //
      //  }
         if (!InRange(_targetEvadeRange, _currentTarget))
        {
            _currentState = EnemyState.Patrol;
        }
    }

    protected GameObject GetTarget()
    {
        var colliders = Physics2D.OverlapCircleAll(transform.position, _detectRange);

        GameObject closestTarget = null;
        GameObject playerTarget = null;
        GameObject currentTarget = null;

        float playerDistance = 0;
        float targetDistance = float.MaxValue;
        float closestTargetDistance = 0;


        foreach (var collider in colliders)
        {

            if (collider.gameObject == gameObject)
                continue;

            targetDistance = Vector2.Distance(collider.gameObject.transform.position, transform.position);

            if (collider.gameObject.CompareTag("Player"))
            {
                playerTarget = collider.gameObject;
                playerDistance = targetDistance;
            }
                
            if (targetDistance < closestTargetDistance)
            {
                closestTarget = collider.gameObject;
                closestTargetDistance = targetDistance;
            }
        }

            if (closestTargetDistance < _targetEvadeRange)
            {
                currentTarget = closestTarget;
                _currentState = EnemyState.EvadeTarget;
            }
            else if (playerTarget != null)
            {
                currentTarget = playerTarget;
                _attackRange = _playerAttackRange;
            }
            else
            {
                currentTarget = null;
            }


            return currentTarget;
    }
}
