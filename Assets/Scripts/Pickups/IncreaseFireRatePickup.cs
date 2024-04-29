using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreaseFireRatePickup : PickUp
{
    [SerializeField] private float _powerUpTimerLength = 10;
    [SerializeField] private float _fireRateIncreaseFactor = 5f;
    
   // private CooldownTimer _powerUpCooldownTimer;
    private WeaponSystem weaponSystem;

    private bool _powerUpUsed;
    private float _currentCooldown;


   // void Start()
  //  {
     //   _powerUpCooldownTimer = new CooldownTimer(_powerUpTimerLength);
  //  }

   // private void Update()
  //  {
    //    if (_powerUpCooldownTimer.IsRunning()) 
    //    {
   //         _powerUpCooldownTimer.UpdateTimer(Time.deltaTime);
    //    }
  //      else if (!_powerUpCooldownTimer.IsRunning() && _powerUpUsed)
   //     {
  //          weaponSystem.PrimaryWeaponCooldown = _currentCooldown;
  //          Destroy(gameObject);
  //      }    
 //   }

    public override void CollectObject(Unit unit)
    {
        if (_powerUpUsed || (weaponSystem = unit.GetComponentInChildren<WeaponSystem>()) == null)
            return;

        _powerUpUsed = true;

        _currentCooldown = weaponSystem.PrimaryWeaponCooldown;
         
        weaponSystem.PrimaryWeaponCooldown /=_fireRateIncreaseFactor;

        CoroutineManager.Instance.Coroutine(PowerUpCoolDown(_currentCooldown, _powerUpTimerLength));

        Destroy(gameObject);
    }

    IEnumerator PowerUpCoolDown(float currentCooldown,  float timerLength)
    {
        yield return new WaitForSeconds(timerLength);

        weaponSystem.PrimaryWeaponCooldown = currentCooldown;
    }
}
