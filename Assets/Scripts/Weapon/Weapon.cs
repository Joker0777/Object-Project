using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Weapon : ScriptableObject
{
    public abstract void ShootWeapon(Vector2 position, Vector2 direction, Quaternion rotation, string targetTag, Unit userUnit);  
}
