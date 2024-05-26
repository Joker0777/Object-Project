
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WeaponSystem : UnitSystems 
{
    [SerializeField]protected List<Weapon> _weaponList = new List<Weapon>();
    [SerializeField]protected WeaponSpawnPoints[] _weaponSpawnPoints;
    [SerializeField]protected Transform _projectileParent;
  
    [SerializeField]protected float _currentWeaponCooldown;  
    [SerializeField]protected string _weaponTarget;

    protected Weapon _currentWeapon;
    protected Transform[] _curremtWeaponSpawnPoints;
    protected int _currentWeaponIndex = 0;
    protected int _weaponSpawnPointIndex;
    protected Timer _cooldownTimer;
    protected float _fireRateIncreaseFactor = 1f;

    public float WeaponCooldown
    {
        get { return _currentWeaponCooldown; }
        set
        {
            _currentWeaponCooldown = value / _fireRateIncreaseFactor;
            _cooldownTimer.TimerDuration = _currentWeaponCooldown;
        }
    }

    public float FireRateIncreaseFactor
    {
        set
        {
            _fireRateIncreaseFactor = value;
            WeaponCooldown = _currentWeaponCooldown;
        }
        get { return _fireRateIncreaseFactor; }
    }

    public int WeaponCount
    {
        get {return _weaponList.Count; }
    }

    protected override void Start()
    {
        base.Start();
        _cooldownTimer = new Timer(_currentWeaponCooldown);
        
        if (_weaponList.Count != 0)
        {
           _currentWeapon = _weaponList[_currentWeaponIndex];
        }
    }

    public virtual void AddWeapon(Weapon addedWeapon)
    {
        if (addedWeapon != null && !_weaponList.Contains(addedWeapon))
        {
            _weaponList.Add(addedWeapon);
            _currentWeapon = _weaponList[_currentWeaponIndex];
        }   
    }

    
    public void FireWeapon()
    {
        if (_cooldownTimer.IsRunningCoroutine) return;
    
        if (_currentWeapon != null)
        {
            SetUpWeapon(_currentWeapon.WeaponType);

            _weaponSpawnPointIndex = (_weaponSpawnPointIndex +1) %_curremtWeaponSpawnPoints.Length;
           
            Transform weaponSpawn = _curremtWeaponSpawnPoints[_weaponSpawnPointIndex];

            _currentWeapon.ShootWeapon(weaponSpawn.position, weaponSpawn.up,
                                              weaponSpawn.rotation, _weaponTarget,_projectileParent);

            _cooldownTimer.StartTimerCoroutine();
        }
    }


    public virtual void SwitchWeapon(int weaponListNumber)
    {
        if(weaponListNumber >= 0 && weaponListNumber < _weaponList.Count)
        {
            _currentWeapon = _weaponList[weaponListNumber];        
        }
    }


    protected void SetUpWeapon(string weaponType)
    {
        foreach(var weaponSpawn in _weaponSpawnPoints)
        {
            if(weaponSpawn.WeaponTypeTag == weaponType)
            {
                _curremtWeaponSpawnPoints = weaponSpawn.SpawnLocations;
                WeaponCooldown = _currentWeapon.WeaponCooldown;
                return;
            }
        }
    }
}
