using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Weapon : ScriptableObject
{
    [Header("Weapon Setup")]
    [SerializeField] protected string _weaponObjectPoolTag;
    [SerializeField] protected float _weaponCooldown;
    public string WeaponType
    {
        get { return _weaponObjectPoolTag; }
    }

    public float WeaponCooldown
    {
        get { return _weaponCooldown; }
    }
    public abstract void ShootWeapon(Vector2 position, Vector2 direction, Quaternion rotation, string targetTag, Transform parent);  
}
