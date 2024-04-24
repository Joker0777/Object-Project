using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : PickUp
{
    [SerializeField] private Weapon _weaponCollected;

    public override void CollectObject(Unit unit)
    {
        WeaponSystem weaponSystem = unit.GetComponentInChildren<WeaponSystem>();
       
        weaponSystem.AddPrimary(_weaponCollected);
        
        base.CollectObject(unit);
    }
}
