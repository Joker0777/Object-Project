using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponSystem : WeaponSystem
{
    Weapon secondaryWeapon;

    [SerializeField] Transform _secondaryWeaponSpawnPoint;

    [SerializeField] protected float _secondaryWeaponCooldown;

    [SerializeField] protected int _maxSecondaryAmmo = 3;

    [SerializeField] protected Weapon _secondaryPrefab;

    [SerializeField] protected GameObject[] _weaponVisials;

    private int _secondaryWeaponAmmo = 0;

    private Timer _secondaryCooldownTimer;



    protected override void Start()
    {
        base.Start();

        _eventManager.OnUIChange?.Invoke(UIElementType.Bomb, _secondaryWeaponAmmo.ToString());

        secondaryWeapon = _secondaryPrefab;

        _secondaryCooldownTimer = new Timer(_secondaryWeaponCooldown);
 
        _eventManager.OnUIChange?.Invoke(UIElementType.Weapon, _currentWeapon.name);
        
    }

    public override void AddWeapon(Weapon addedWeapon)
    {
        if (addedWeapon != null && !_weaponList.Contains(addedWeapon))
        {
            _weaponList.Add(addedWeapon);
            _currentWeapon = _weaponList[_currentWeaponIndex];

            foreach (var weapon in _weaponVisials)
            {
                if (weapon != null && addedWeapon.WeaponType == weapon.name)
                {
                    weapon.SetActive(true);
                }
            }
        }
    }

    public void AddSecondaryAmmo()
    {
        if (_secondaryWeaponAmmo < _maxSecondaryAmmo)
        {
            _secondaryWeaponAmmo++;
            _eventManager.OnUIChange?.Invoke(UIElementType.Bomb, _secondaryWeaponAmmo.ToString());
        }
    }

    public void FireSecondary()
    {
        if (_secondaryCooldownTimer.IsRunningCoroutine || _secondaryWeaponAmmo <= 0) return;

        if (secondaryWeapon != null)
        {
            secondaryWeapon.ShootWeapon(_secondaryWeaponSpawnPoint.transform.position, _secondaryWeaponSpawnPoint.transform.up, _secondaryWeaponSpawnPoint.rotation,
                                        _weaponTarget, _projectileParent);
            _secondaryCooldownTimer.StartTimerCoroutine();
            _secondaryWeaponAmmo--;
            _eventManager.OnUIChange?.Invoke(UIElementType.Bomb, _secondaryWeaponAmmo.ToString());
        }
    }

    public override void SwitchWeapon(int swithchDirection)
    {
        if (_weaponList.Count == 0)
        {
            return;
        }
        if (swithchDirection > 0)
        {
            _currentWeaponIndex = (_currentWeaponIndex + 1) % _weaponList.Count;
            _currentWeapon = _weaponList[_currentWeaponIndex];
        }
        else
        {
            _currentWeaponIndex = (_currentWeaponIndex - 1 + _weaponList.Count) % _weaponList.Count;
            _currentWeapon = _weaponList[_currentWeaponIndex];
        }

        _eventManager.OnUIChange?.Invoke(UIElementType.Weapon, _currentWeapon.name);
    }
}
