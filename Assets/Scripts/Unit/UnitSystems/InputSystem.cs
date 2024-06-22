using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InputSystem : UnitSystems
{
    protected float _horizontalInput;
    protected float _verticalInput;
    protected float _scrolledInput;

    private IMovable movement;
    private PlayerWeaponSystem weapon;

    Vector2 _distanceToFace;
    float _angleToTurn;

    protected override void Start()
    {
        base.Start();
        movement = GetComponentInChildren<IMovable>();
        weapon = GetComponentInChildren<PlayerWeaponSystem>();

        if (movement == null)
        {
            Debug.Log("Movment not assigned or does not implement IMovable");
        }

        if (weapon == null)
        {
            Debug.Log("Weapon not assigned or does not implement IWeapon");
        }
    }


    void Update()
    {
        _horizontalInput = Input.GetAxis("Horizontal");
        _verticalInput = Input.GetAxis("Vertical");
        _scrolledInput = Input.GetAxis("Mouse ScrollWheel");

        Vector2 moveInput = new Vector2(_horizontalInput, _verticalInput);

        unit.EventManager.IsThrusting(_verticalInput > 0 ? true : false);


        _distanceToFace = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _angleToTurn = Mathf.Atan2(_distanceToFace.y - unit.transform.position.y, _distanceToFace.x - unit.transform.position.x) * Mathf.Rad2Deg;

        //Movement
        movement?.Move(moveInput, _angleToTurn);


        //Primary Weapon
        if (Input.GetKey(KeyCode.Space) || Input.GetMouseButton(0))
        {
              weapon?.FireWeapon();
        }

        //Secondary Weapon
        if (Input.GetKeyDown(KeyCode.LeftAlt) || Input.GetMouseButtonDown(1))
        {
            weapon?.FireSecondary();
        }

        //Change Primary Weapon
        if(_scrolledInput > 0 || Input.GetKeyDown(KeyCode.E))
        {
            weapon?.SwitchWeapon(1);        
        }

        if(_scrolledInput < 0 || Input.GetKeyDown(KeyCode.Q))
        {
            weapon?.SwitchWeapon(-1);
        }



    }
}
