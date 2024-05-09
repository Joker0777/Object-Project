using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpManager : MonoBehaviour
{

    public static PickUpManager instance;

    private Unit _playerUnit;
    private PickupType pickupType;

    //weapon pickups   
    [SerializeField]private Weapon _laserWeapon;
    [SerializeField] private Weapon _missleWeapon;
    [SerializeField] private Weapon _plasmaWeapon;

    //Health pickup
    [SerializeField] int _healthAmount;

    //PowerUp
    [SerializeField] protected float _powerUpTimerLength = 10;
    private IncreaseFireRatePowerUp _increaseFireRatePowerUp;
    private IncreaseSpeedPowerUP _increaseSpeedPowerUp;


    private Timer _PowerUpTimer;
    private float _timeRemaining;
    private bool _powerUpTimerRunning;
    private Action<Unit> _currentDeactivateFunction;

    private WeaponSystem weaponSystem;
    private MovmentSystem movmentSystem;
    private ShieldSystem shieldSystem;

    private void Awake()
    {
        instance = this;
    }


    private void Start()
    {
        _increaseFireRatePowerUp = new IncreaseFireRatePowerUp();
        _increaseSpeedPowerUp = new IncreaseSpeedPowerUP();

        _PowerUpTimer = new Timer(_powerUpTimerLength);
    }

    private void Update()
    {
        if (_powerUpTimerRunning)
        {
            _PowerUpTimer.UpdateTimerBasic(Time.deltaTime);

            _playerUnit.EventManager.OnUIChange?.Invoke(UIElementType.PickUpTimerUI, Mathf.CeilToInt(_PowerUpTimer.TimeRemaining).ToString());

            if (!_PowerUpTimer.IsRunningBasic())
            {
                _powerUpTimerRunning = false;
                _currentDeactivateFunction(_playerUnit);
                _playerUnit.EventManager.OnUIChange?.Invoke(UIElementType.PickUpTimerUI, "0");
            }
        }
    }

    public void CollectObject(Unit unit, PickupType pickupType)
    {
        Debug.Log("At the start of collect object in pickup manager");

        _playerUnit = unit;

        if (_playerUnit == null) return;

     

  
       
        switch (pickupType)
        {
            case PickupType.ExplosivePickup:
                Debug.Log("Explosive");
                CollectExplosive();
                break;

            case PickupType.HealthPickup:
                Debug.Log("Health");
                CollectHealth();
                break;

            case PickupType.ShieldPickup:

                break;

            case PickupType.MissleWeaponPickup:
                
                CollectWeapon(_missleWeapon);
                break;

            case PickupType.LaserWeaponPickup:
                Debug.Log("Laser");
                CollectWeapon(_laserWeapon);
                break;

            case PickupType.PlamsaWeaponPickup:
                
                CollectWeapon(_plasmaWeapon);
                break;

            case PickupType.ShieldPowerUpPickup:
                
                break;

            case PickupType.IncreaseFireRatePickup:
                Debug.Log("Fire Rate");
                CollectPowerUp(_increaseFireRatePowerUp.ActivatePowerUp, _increaseFireRatePowerUp.DeactivatePowerUp);   
                break;

            case PickupType.IncreaseSpeedPickup:
                Debug.Log("Speed Increase");
                CollectPowerUp(_increaseSpeedPowerUp.ActivatePowerUp, _increaseSpeedPowerUp.DeactivatePowerUp);
                break;
        }
    }



    private void CollectWeapon(Weapon weapon)
    {
        _playerUnit.GetComponentInChildren<PlayerWeaponSystem>().AddPrimary(weapon);
    }

    private void CollectExplosive()
    {
        _playerUnit.GetComponentInChildren<PlayerWeaponSystem>().AddSecondaryAmmo();
    }

    private void CollectHealth()
    {
        _playerUnit.HealthIncrease(_healthAmount);
    }

    private void CollectPowerUp(Action<Unit> activatePowerUpFunction, Action<Unit> deactivatePowerUpFuntion)
    {
        if (activatePowerUpFunction == null) return;
        if (_powerUpTimerRunning && _currentDeactivateFunction != null)
        {
            _currentDeactivateFunction(_playerUnit);
            _PowerUpTimer.StopTimerBasic();
            _powerUpTimerRunning = false;
        }
              
        _currentDeactivateFunction = deactivatePowerUpFuntion;
        
        activatePowerUpFunction(_playerUnit);

        _PowerUpTimer.StartTimerBasic();

        _powerUpTimerRunning = true;

    }
}
