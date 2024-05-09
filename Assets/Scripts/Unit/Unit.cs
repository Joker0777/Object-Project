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

    [SerializeField] EventManager _eventManager;
    [SerializeField] UnitType _unitType;
    [SerializeField] private int _maxHealth = 100;

    public UnitType UnitType {  get { return _unitType; } }

    public EventManager EventManager { get { return _eventManager; } }

    protected virtual void Start()
    {
        _healthSystem = ScriptableObject.CreateInstance<HealthSystem>();
        _healthSystem.CurrentHealth = _maxHealth;
        _healthSystem.MaxHealth = _maxHealth;

        _eventManager.OnUIChange?.Invoke(UIElementType.HealthUI, GetHealth().ToString());
    }

    private void Update()
    {
        if (_healthSystem.IsDestroyed)
        {
            UnitDestroyed();
        }
    }

    public virtual void DamageTaken(int damage)
    {
        _healthSystem.DecreaseHealth(damage);
        _eventManager.OnUnitHealthChanged ?.Invoke(_healthSystem.CurrentHealth);
        if(UnitType == UnitType.Player)
        {
            _eventManager.OnUIChange?.Invoke(UIElementType.HealthUI, GetHealth().ToString());
        }
    }

    public virtual void HealthIncrease(int health)
    {
        _healthSystem.IncreaseHealth(health);
        _eventManager.OnUnitHealthChanged?.Invoke(_healthSystem.CurrentHealth);

        if (UnitType == UnitType.Player)
        {
            _eventManager.OnUIChange?.Invoke(UIElementType.HealthUI, GetHealth().ToString());
        }
    }

    public virtual int GetHealth()
    {
        return _healthSystem.CurrentHealth;
    }

    protected virtual void UnitDestroyed()
    {
        _eventManager.OnUnitDestroyed?.Invoke(UnitType);
        Destroy(gameObject);
    }
}
