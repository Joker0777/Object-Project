using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpManager : MonoBehaviour
{

    public static PickUpManager instance;


    [SerializeField] private PickupSpawner _pickupSpawner;
    [SerializeField] private Unit _playerUnit;

    //weapon pickups   
    [SerializeField]private Weapon _laserWeapon;
    [SerializeField] private Weapon _missleWeapon;
    [SerializeField] private Weapon _plasmaWeapon;

    //health pickup
    [SerializeField] int _healthAmount;

    //powerUp pickups
    private IncreaseFireRatePowerUp _increaseFireRatePowerUp;
    private IncreaseSpeedPowerUP _increaseSpeedPowerUp;
    private Action<Unit> _currentDeactivateFunction;

    //powerup timer
    [SerializeField] protected float _powerUpTimerLength = 10;
    private Timer _PowerUpTimer;
    private float _timeRemaining;
    private bool _powerUpTimerRunning;

    //player systems references
    private PlayerWeaponSystem playerWeaponSystem;
    private MovmentSystem movmentSystem;
    private ShieldSystem shieldSystem;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        _increaseFireRatePowerUp = new IncreaseFireRatePowerUp();
        _increaseSpeedPowerUp = new IncreaseSpeedPowerUP();

        _PowerUpTimer = new Timer(_powerUpTimerLength);

        playerWeaponSystem = _playerUnit.GetComponentInChildren<PlayerWeaponSystem>();
        movmentSystem = _playerUnit.GetComponentInChildren<MovmentSystem>();

        _playerUnit.EventManager.OnUIChange?.Invoke(UIElementType.PickUpTimer, "0");


    }

    private void Update()
    {
        if (_powerUpTimerRunning)
        {
            _PowerUpTimer.UpdateTimerBasic(Time.deltaTime);

            _playerUnit.EventManager.OnUIChange?.Invoke(UIElementType.PickUpTimer, Mathf.CeilToInt(_PowerUpTimer.TimeRemaining).ToString());

            if (!_PowerUpTimer.IsRunningBasic())
            {
                _powerUpTimerRunning = false;
                _currentDeactivateFunction(_playerUnit);
                _playerUnit.EventManager.OnUIChange?.Invoke(UIElementType.PickUpTimer, "0");
            }
        }
    }

    public void CollectObject(Unit unit, PickupType pickupType)
    {
        _playerUnit = unit;

        if (_playerUnit == null) return;
    
        switch (pickupType)
        {
            case PickupType.ExplosivePickup:
                CollectExplosive();
                break;

            case PickupType.HealthPickup:
                CollectHealth();
                break;

            case PickupType.ShieldPickup://to be implemented

                break;

            case PickupType.MissleWeaponPickup:            
                CollectWeapon(_missleWeapon);
                break;

            case PickupType.LaserWeaponPickup:
                CollectWeapon(_laserWeapon);
                break;

            case PickupType.PlamsaWeaponPickup:             
                CollectWeapon(_plasmaWeapon);
                break;

            case PickupType.ShieldPowerUpPickup://to be implemented
                
                break;

            case PickupType.IncreaseFireRatePickup:
                CollectPowerUp(_increaseFireRatePowerUp.ActivatePowerUp, _increaseFireRatePowerUp.DeactivatePowerUp);   
                break;

            case PickupType.IncreaseSpeedPickup:
                CollectPowerUp(_increaseSpeedPowerUp.ActivatePowerUp, _increaseSpeedPowerUp.DeactivatePowerUp);
                break;
        }
    }


    private void CollectWeapon(Weapon weapon)
    {
        if(playerWeaponSystem != null)
        {
            playerWeaponSystem.AddWeapon(weapon);
        }
    }

    private void CollectExplosive()
    {
        if(playerWeaponSystem != null)
        {
            playerWeaponSystem.AddSecondaryAmmo();
        }
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

    public void DestoyAllPickUps()
    {
        List<GameObject> spawnedPickUps = _pickupSpawner.SpawnedPickUps;

        if(spawnedPickUps != null && spawnedPickUps.Count > 0)
        {
            foreach (GameObject pickUp in spawnedPickUps)
            {
                if (pickUp != null)
                {
                    Destroy(pickUp.gameObject);
                }
            }
            spawnedPickUps.Clear();
        }
    }
}
