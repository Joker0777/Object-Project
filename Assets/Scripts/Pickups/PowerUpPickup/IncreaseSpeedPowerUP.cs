using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreaseSpeedPowerUP
{
    private float _speedIncreaseFactor;

    private float _currentSpeed;

    protected MovmentSystem movmentSystem;


    public IncreaseSpeedPowerUP(float speedIncreaseFactor = 1.5f) {  _speedIncreaseFactor = speedIncreaseFactor; }

    public void ActivatePowerUp(Unit unit)
    {
        if ((movmentSystem = unit.GetComponentInChildren<MovmentSystem>()) == null)
            return;

        _currentSpeed = movmentSystem.Speed;

        movmentSystem.Speed *= _speedIncreaseFactor;
    }

    public void DeactivatePowerUp(Unit unit)
    {
        if (movmentSystem != null)
        {
            movmentSystem.Speed = _currentSpeed;
        }
    }
}
