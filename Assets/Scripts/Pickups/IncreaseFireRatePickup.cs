using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreaseFireRatePickup : PickUp
{
    [SerializeField] private float _powerUpTimerLength = 10;
    [SerializeField] private float _fireRateIncreaseFactor = 5f;
    
    private CooldownTimer _powerUpCooldownTimer;
    private WeaponSystem weaponSystem;

    private bool _powerUpUsed;
    private float _currentCooldown;


    void Start()
    {
        _powerUpCooldownTimer = new CooldownTimer(_powerUpTimerLength);
    }

    private void Update()
    {
        if (_powerUpCooldownTimer.IsRunning()) 
        {
            _powerUpCooldownTimer.UpdateTimer(Time.deltaTime);
        }
        else if (!_powerUpCooldownTimer.IsRunning() && _powerUpUsed)
        {
            weaponSystem.PrimaryWeaponCooldown = _currentCooldown;
            Destroy(gameObject);
        }    
    }

    public override void CollectObject(Unit unit)
    {
        weaponSystem = unit.GetComponentInChildren<WeaponSystem>();

        if (weaponSystem != null && !_powerUpUsed) 
        {
           
            _currentCooldown = weaponSystem.PrimaryWeaponCooldown;
            _powerUpCooldownTimer.StartTimer();
            _powerUpUsed = true;
            weaponSystem.PrimaryWeaponCooldown = _currentCooldown/_fireRateIncreaseFactor;
        }

 
    }
}
