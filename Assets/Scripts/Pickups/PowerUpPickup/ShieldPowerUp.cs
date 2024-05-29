using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldPowerUp : PowerUpPickUp
{

    protected ShieldSystem _shieldSystem;


    public override void ActivatePowerUp(Unit unit)
    {     
        if ((_shieldSystem = unit.GetComponentInChildren<ShieldSystem>(true)) == null)
            return;

        _shieldSystem.gameObject.SetActive(true);
        _shieldSystem.ActivateShield();
    }

    public override void DeactivatePowerUp(Unit unit)
    {
        if (_shieldSystem != null)
        {
            _shieldSystem?.DisableShield();
        }
    }
}
