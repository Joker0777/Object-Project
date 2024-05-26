using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    [SerializeField] PickupType pickupType;
    [SerializeField] string unitTag = "Player";
    [SerializeField] float _pickupTimerLength = 15f;

    private Timer _pickUpTimer;

    private void Awake()
    {
        _pickUpTimer = new Timer(_pickupTimerLength);
    }


    private void Start()
    {
        _pickUpTimer.StartTimerBasic();
    }

    private void Update()
    {
        _pickUpTimer.UpdateTimerBasic(Time.deltaTime);

        if (!_pickUpTimer.IsRunningBasic())
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision == null) return;

        Unit unit = collision.GetComponent<Unit>();
        if (unit != null && unit.CompareTag(unitTag))
        {
           PickUpManager.instance.CollectObject(unit, pickupType);
           Destroy(gameObject);
        }       
    }
}
