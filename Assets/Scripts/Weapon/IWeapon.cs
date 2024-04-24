using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeapon
{
    public void FirePrimary();
    public void FireSecondary();
    public void SwitchPrimary(int swithchDirection);

}
