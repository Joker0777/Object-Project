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
    public int healthTest;

    [SerializeField] EventManager _eventManager;
    [SerializeField] UnitType _unitType;
    [SerializeField] private int _maxHealth = 100;


    

    public UnitType UnitType 
    {  get { return _unitType; } }

    protected virtual void Start()
    {
        _healthSystem = ScriptableObject.CreateInstance<HealthSystem>();
        _healthSystem.CurrentHealth = _maxHealth;
        _healthSystem.MaxHealth = _maxHealth;       
    }

    private void Update()
    {
        if (_healthSystem.IsDestroyed)
        {
            UnitDestroyed();
        }
        healthTest = GetHealth();
    }

    public virtual void DamageTaken(int damage)
    {
        _healthSystem.DecreaseHealth(damage);
         _eventManager.HealthChangeEvents.OnUnitHealthChanged ?.Invoke(_healthSystem.CurrentHealth);
    }

    public virtual void HealthIncrease(int health)
    {
        _eventManager.HealthChangeEvents.OnUnitHealthChanged?.Invoke(_healthSystem.IncreaseHealth(health));
    }

    public virtual int GetHealth()
    {
        return _healthSystem.CurrentHealth;
    }

    protected virtual void UnitDestroyed()
    {
        _eventManager.UnitDieEvents.OnUnitDestroyed?.Invoke();
        Destroy(gameObject);
    }

    private void OnEnable()
    {
        InitializeSystems(this);
    }

    public void InitializeSystems(Unit parent)
    {

        IInitialize[] initialize = GetComponentsInChildren<IInitialize>();

        foreach (IInitialize init in initialize) 
        { 
            init?.InitializeSystem(parent);
        }
    }

}
