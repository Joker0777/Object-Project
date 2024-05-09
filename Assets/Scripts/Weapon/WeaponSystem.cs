
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WeaponSystem : UnitSystems 
{




    protected List<Weapon> primaryWeaponList = new List<Weapon>();
    

    protected Weapon primaryWeapon;
   
   
    [SerializeField]protected WeaponSpawnPoint[] _weaponSpawnPoints;

    [SerializeField] protected Transform _projectileParent;

    //starting weapons
    [SerializeField] protected Weapon _primaryWeapon;//set the default weapon


    //weapon cool down timer length
    [SerializeField]protected float _primaryWeaponCooldown;

    
    [SerializeField]protected string _weaponTarget;


    protected Weapon _currentPrimaryWeapon;
    protected Transform[] _primaryWeaponSpawnPoints;
    protected int _currentWeaponIndex = 0;
    protected int _primaryWeaponSpawnPointIndex;


    protected Timer _primaryCooldownTimer;




    protected override void Start()
    {
        base.Start();
        _primaryCooldownTimer = new Timer(_primaryWeaponCooldown);

        
        primaryWeaponList.Add(_primaryWeapon);
        if (primaryWeaponList.Count != 0)
        {
           _currentPrimaryWeapon = primaryWeaponList[_currentWeaponIndex];

           _eventManager.OnUIChange?.Invoke(UIElementType.WeaponUI, _primaryWeapon.name);
        }


    }

    public void AddPrimary(Weapon addedWeapon)
    {
        primaryWeaponList.Add(addedWeapon);
        _currentPrimaryWeapon = primaryWeaponList[_currentWeaponIndex];      
    }



    public void FirePrimary()
    {
        if (_primaryCooldownTimer.IsRunningCoroutine) return;
    
        if (_currentPrimaryWeapon != null)
        {
            SetUpWeapon(_currentPrimaryWeapon.WeaponType);

            _primaryWeaponSpawnPointIndex = (_primaryWeaponSpawnPointIndex +1) %_primaryWeaponSpawnPoints.Length;
           
            Transform weaponSpawn = _primaryWeaponSpawnPoints[_primaryWeaponSpawnPointIndex];

            _currentPrimaryWeapon.ShootWeapon(weaponSpawn.position, weaponSpawn.up,
                                              weaponSpawn.rotation, _weaponTarget,_projectileParent);

           // Debug.Log(_primaryWeaponSpawnPointIndex);

            _primaryCooldownTimer.StartTimerCoroutine();
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

        _eventManager.OnUIChange?.Invoke(UIElementType.WeaponUI, _currentPrimaryWeapon.name);
    }

    protected void SetUpWeapon(string weaponType)
    {
        foreach(var weaponSpawn in _weaponSpawnPoints)
        {
            if(weaponSpawn.WeaponTypeTag == weaponType)
            {
                _primaryWeaponSpawnPoints = weaponSpawn.SpawnLocations;
                _primaryWeaponCooldown = _currentPrimaryWeapon.WeaponCooldown;
            }
        }
        

    }
}
