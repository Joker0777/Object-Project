using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviourSystem : UnitSystems
{

    protected CircleCollider2D _targetRadiusCollider;

    protected IMovable movement;
    private WeaponSystem weapon;

    [SerializeField] protected float _attackRange = 3;
    [SerializeField] protected float _detectRange = 7;
    [SerializeField] protected float _stopRange = 1;


    protected EnemyState _currentState;

    protected GameObject _target;

    protected float _angleToTurn;
    protected Vector2 moveInput;
    protected Vector2 directionToCurrentTarget;

    protected override void Awake()
    {
        base.Awake();
        movement = GetComponentInChildren<IMovable>();
        weapon = GetComponentInChildren<WeaponSystem>();

        _targetRadiusCollider = GetComponent<CircleCollider2D>();

        if (movement == null)
        {
            Debug.Log("Movment not assigned or does not implement IMovable");
        }

        if (weapon == null)
        {
            Debug.Log("Weapon not assigned or does not implement IWeapon");
        }


        _targetRadiusCollider.radius = _detectRange;

        _currentState = EnemyState.Patrol;

        moveInput = Vector2.zero;

        directionToCurrentTarget = Vector2.zero;
    }
   


    protected virtual void Update()
    {
        _angleToTurn = Mathf.Atan2(directionToCurrentTarget.y, directionToCurrentTarget.x) * Mathf.Rad2Deg;

        movement?.Move(moveInput, _angleToTurn);

        switch (_currentState)
        {
            case EnemyState.Patrol:
                Patrol();
                break;

            case EnemyState.Detect:
                DetectTarget();
                break;

            case EnemyState.Attack:
                Attack();
                break;
        }
    }

    protected bool InRange(float range)
    {
        if(_target != null && Vector2.Distance(_target.transform.position,transform.position) < range)
        {
            return true;
        }
        return false;
    }

    protected void FollowTarget()
    {
        if (_target != null) 
        {
            if (InRange(_stopRange))
            {
                moveInput = Vector2.zero;
            }
            else
            {
                moveInput = Vector2.up;
            }

            directionToCurrentTarget = (_target.transform.position - transform.position).normalized;          
        }
    }
  
    protected void Patrol()
    {
        moveInput = Vector2.zero;
    }

    protected void Attack()
    {
        
        FollowTarget();

        weapon?.FirePrimary();

        if(!InRange(_attackRange) ) 
        {
            _currentState = EnemyState.Detect;
        }
    }

    protected  void DetectTarget()
    {
        FollowTarget();

        if(InRange(_attackRange)) 
        {
            _currentState = EnemyState.Attack;
        }
    }

    protected void OnTriggerStay2D(Collider2D collision)
    {       
        if (collision.gameObject.CompareTag("Player"))
        {
            _target = collision.gameObject;
            _currentState = EnemyState.Detect;      
        }
    }

    protected void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _currentState = EnemyState.Patrol;
        }
    }
}
