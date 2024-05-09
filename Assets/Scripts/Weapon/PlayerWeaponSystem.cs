using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponSystem : WeaponSystem
{
    Weapon secondaryWeapon;

    [SerializeField] Transform _secondaryWeaponSpawnPoint;

    [SerializeField] protected float _secondaryWeaponCooldown;

    [SerializeField] protected int _maxSecondaryAmmo = 3;

    [SerializeField] protected Weapon _secondaryPrefab;//set the secondary weapon

    private int _secondaryWeaponAmmo = 0;
    private float _fireRateIncreaseFactor = 1f;

    private Timer _secondaryCooldownTimer;


    public float PrimaryWeaponCooldown
    {
        get { return _primaryWeaponCooldown; }
        set
        {
            _primaryWeaponCooldown = value / _fireRateIncreaseFactor;
            _primaryCooldownTimer.TimerDuration = value;
        }
    }

    public float FireRateIncreaseFactor
    {
        set
        {
            _fireRateIncreaseFactor = value;
            PrimaryWeaponCooldown = _primaryWeaponCooldown;
        }
        get { return _fireRateIncreaseFactor; }
    }

    protected override void Start()
    {
        base.Start();

        _eventManager.OnUIChange?.Invoke(UIElementType.BombUI, _secondaryWeaponAmmo.ToString());

        secondaryWeapon = _secondaryPrefab;
    }


    public void AddSecondaryAmmo()
    {
        if (_secondaryWeaponAmmo < _maxSecondaryAmmo)
        {
            _secondaryWeaponAmmo++;
            _eventManager.OnUIChange?.Invoke(UIElementType.BombUI, _secondaryWeaponAmmo.ToString());
        }
    }

    public void FireSecondary()
    {
        if (_secondaryCooldownTimer.IsRunningCoroutine || _secondaryWeaponAmmo <= 0) return;


        if (secondaryWeapon != null)
        {
            secondaryWeapon.ShootWeapon(_secondaryWeaponSpawnPoint.transform.position, -_secondaryWeaponSpawnPoint.transform.up, _secondaryWeaponSpawnPoint.rotation,
                                        _weaponTarget, _projectileParent);
            _secondaryCooldownTimer.StartTimerCoroutine();
            _secondaryWeaponAmmo--;
        }
    }
}
