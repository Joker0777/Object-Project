
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WeaponSystem : UnitSystems, IWeapon 
{
 
    private List<Weapon> primaryWeaponList = new List<Weapon>();
    
    Weapon secondaryWeapon;
    Weapon primaryWeapon;
   
   
    [SerializeField] Transform _primaryWeaponSpawnPoint;
    [SerializeField] Transform _secondaryWeaponSpawnPoint;

    //starting weapons
    [SerializeField] Weapon _primaryWeapon;//set the default weapon
    [SerializeField] Weapon _secondaryPrefab;//set the secondary weapon

    //weapon cool down timer length
    [SerializeField]protected float _primaryWeaponCooldown;
    [SerializeField]protected float _secondaryWeaponCooldown;
    
    [SerializeField]protected string _weaponTarget;
    [SerializeField] protected int _maxSecondaryAmmo = 3;

    private Weapon _currentPrimaryWeapon;
    private int _currentWeaponIndex = 0;
    private int _secondaryWeaponAmmo = 1;

    private CooldownTimer _primaryCooldownTimer;
    private CooldownTimer _secondaryCooldownTimer;

    
    public float PrimaryWeaponCooldown
    {
        get { return _primaryWeaponCooldown;}
        set 
        { 
            _primaryWeaponCooldown = value;
            _primaryCooldownTimer.TimerDuration = value;
        }
    }

    public void Start()
    {
        _primaryCooldownTimer = new CooldownTimer(_primaryWeaponCooldown);
        _secondaryCooldownTimer = new CooldownTimer(_secondaryWeaponCooldown);
        
        primaryWeaponList.Add(_primaryWeapon);
        if (primaryWeaponList.Count != 0)
        {
           _currentPrimaryWeapon = primaryWeaponList[_currentWeaponIndex];      
        }

        secondaryWeapon = _secondaryPrefab;
    }

    private void Update()
    {
        if(_primaryCooldownTimer.IsRunning()) 
        { 
            _primaryCooldownTimer.UpdateTimer(Time.deltaTime);
        }

        if(_secondaryCooldownTimer.IsRunning()) 
        { 
            _secondaryCooldownTimer.UpdateTimer(Time.deltaTime);
        }    
    }

    public void AddPrimary(Weapon addedWeapon)
    {
        primaryWeaponList.Add(addedWeapon);
        _currentPrimaryWeapon = primaryWeaponList[_currentWeaponIndex];
        
    }

    public void AddSecondaryAmmo()
    {
        if(_secondaryWeaponAmmo < _maxSecondaryAmmo)
        {
            _secondaryWeaponAmmo++;
        }

    }

    public void FirePrimary()
    {
        if (_primaryCooldownTimer.IsRunning()) return;
    
        if (_currentPrimaryWeapon != null)
        {
            Debug.Log("In Fire Primary");
           
            _currentPrimaryWeapon.ShootWeapon(_primaryWeaponSpawnPoint.transform.position, _primaryWeaponSpawnPoint.transform.up, _primaryWeaponSpawnPoint.rotation,
                                              _weaponTarget,unit);
            _primaryCooldownTimer.StartTimer();
        }
    }

    public void FireSecondary() 
    {
       // if (_secondaryCooldownTimer.IsRunning() || _secondaryWeaponAmmo <= 0) return;
        if (_secondaryCooldownTimer.IsRunning()) return;

        if (secondaryWeapon != null) 
        {
            secondaryWeapon.ShootWeapon(_secondaryWeaponSpawnPoint.transform.position, -_secondaryWeaponSpawnPoint.transform.up, _secondaryWeaponSpawnPoint.rotation,
                                        _weaponTarget, unit);
            _secondaryCooldownTimer.StartTimer();
            _secondaryWeaponAmmo--;
            Debug.Log("Secondary Ammo " + _secondaryWeaponAmmo);
        }    
    }

    public void SwitchPrimary(int swithchDirection)
    {
        if (primaryWeaponList.Count == 0)
        {
            return;
        }
        if (swithchDirection > 0)
        {
            _currentWeaponIndex = (_currentWeaponIndex + 1) % primaryWeaponList.Count;
            _currentPrimaryWeapon = primaryWeaponList[_currentWeaponIndex];
        }
        else
        {
            _currentWeaponIndex = (_currentWeaponIndex - 1 + primaryWeaponList.Count) % primaryWeaponList.Count;
            _currentPrimaryWeapon = primaryWeaponList[_currentWeaponIndex];
        }
    }
}
