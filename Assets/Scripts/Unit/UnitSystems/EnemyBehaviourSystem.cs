using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviourSystem : UnitSystems
{

    private CircleCollider2D _targetRadiusCollider;

    private IMovable movement;
    private IWeapon weapon;

    [SerializeField] private float _attackRange = 3;
    [SerializeField] private float _detectRange = 7;
    [SerializeField] private float _stopRange = 1;


    private EnemyState _currentState;

    private GameObject _target;

    private float _angleToTurn;
    protected Vector2 moveInput;
    private Vector2 directionToCurrentTarget;

    void Start()
    {
        movement = GetComponentInChildren<IMovable>();
        weapon = GetComponentInChildren<IWeapon>();

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


    void Update()
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

    private bool InRange(float range)
    {
        if(_target != null && Vector2.Distance(_target.transform.position,transform.position) < range)
        {
            return true;
        }
        return false;
    }

    private void FollowTarget()
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
  
    private void Patrol()
    {
        moveInput = Vector2.zero;
    }

    private void Attack()
    {
        FollowTarget();

        weapon?.FirePrimary();

        if(!InRange(_attackRange) ) 
        {
            _currentState = EnemyState.Detect;
        }
    }

    private void DetectTarget()
    {
        FollowTarget();

        if(InRange(_attackRange)) 
        {
            _currentState = EnemyState.Attack;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {       
        if (collision.gameObject.CompareTag("Player"))
        {
            _target = collision.gameObject;
            _currentState = EnemyState.Detect;      
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _currentState = EnemyState.Patrol;
        }
    }
}
