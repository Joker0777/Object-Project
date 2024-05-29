using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PowerUpPickUp
{
    public abstract void ActivatePowerUp(Unit unit);
    public abstract void DeactivatePowerUp(Unit unit);
}
