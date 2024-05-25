using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreaseFireRatePowerUp
{
    private float _fireRateIncreaseFactor;

    private float _currentCooldown;

    private float _currentFireRateIncreaseFactor;

    private PlayerWeaponSystem weaponSystem;

    public IncreaseFireRatePowerUp(float fireRateIncreaseFactor = 5f) 
    { 
        _fireRateIncreaseFactor = fireRateIncreaseFactor; 
    }

    public void ActivatePowerUp(Unit unit)
    {
        if ((weaponSystem = unit.GetComponentInChildren<PlayerWeaponSystem>()) == null)
            return;

       
       _currentCooldown = weaponSystem.WeaponCooldown;
       _currentFireRateIncreaseFactor = weaponSystem.FireRateIncreaseFactor;

        weaponSystem.FireRateIncreaseFactor = _fireRateIncreaseFactor;
    }

    public void DeactivatePowerUp(Unit unit)
    {
        if (weaponSystem != null)
        {
            weaponSystem.WeaponCooldown = _currentCooldown;
            weaponSystem.FireRateIncreaseFactor = _currentFireRateIncreaseFactor;
        }
    }
}
