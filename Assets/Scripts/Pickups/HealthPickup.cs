using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : PickUp
{
    [SerializeField] int _healthAmount;
    
    public override void CollectObject(Unit unit)
    {
        unit.HealthIncrease(_healthAmount);

        base.CollectObject(unit);
    }
}
