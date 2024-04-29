using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovmentSystem : UnitSystems, IMovable
{

    [SerializeField] protected float _turnSpeed = 0.1f;
    [SerializeField] protected float _speed = 1f;


    protected float _verticalInput;

    protected float _angleToTurn;
    protected Quaternion _targetRotation;

    protected Rigidbody2D _rigidbody2D;

    protected override void Awake()
    {
        base.Awake();

    }

    private void Start()
    {
        _rigidbody2D = unit.GetComponent<Rigidbody2D>();
    }




    protected virtual void FixedUpdate()
    {
        _rigidbody2D.AddForce(transform.up * _verticalInput * _speed);
        _targetRotation = Quaternion.Euler(0, 0, _angleToTurn - 90f);

        unit.transform.rotation = Quaternion.Slerp(transform.rotation, _targetRotation, _turnSpeed * Time.deltaTime);
    }

    public virtual void Move(Vector2 input, float angleToTurn)
    {
        _verticalInput = input.y;
        _angleToTurn = angleToTurn;
    }
}
