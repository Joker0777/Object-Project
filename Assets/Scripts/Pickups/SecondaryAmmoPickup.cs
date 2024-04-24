using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondaryAmmoPickup : PickUp
{
  
    public override void CollectObject(Unit unit)
    {
        WeaponSystem weaponSystem = unit.GetComponentInChildren<WeaponSystem>();

        weaponSystem.AddSecondaryAmmo();

        base.CollectObject(unit);
    }
}
