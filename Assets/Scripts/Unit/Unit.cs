using System;
using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class Unit : MonoBehaviour, IDamagable
{
    protected HealthSystem _healthSystem;

    [SerializeField] protected EventManager _eventManager;
    [SerializeField] protected UnitType _unitType;
    [SerializeField] protected int _maxHealth = 100;

    public UnitType UnitType {  get { return _unitType; } }

    public EventManager EventManager { get { return _eventManager; } }

    protected virtual void Start()
    {
        _healthSystem = new HealthSystem();
        _healthSystem.CurrentHealth = _maxHealth;
        _healthSystem.MaxHealth = _maxHealth;
    }

    protected void Update()
    {
        if (_healthSystem.IsDestroyed)
        {
            UnitDestroyed();
        }
    }

    protected virtual void UnitDestroyed()
    {
        _eventManager.OnPlayParticleEffect?.Invoke(UnitType.ToString(), transform.position, 1);
        Destroy(gameObject);
    }

    public virtual void DamageTaken(int damage)
    {
        _healthSystem.DecreaseHealth(damage);
    }

    public virtual void HealthIncrease(int health)
    {
        _healthSystem.IncreaseHealth(health);
    }

    public virtual void ResetHealth(int health)
    {
        _healthSystem.CurrentHealth = health;
    }

    public int GetHealth()
    {
        return _healthSystem.CurrentHealth;
    }
}
