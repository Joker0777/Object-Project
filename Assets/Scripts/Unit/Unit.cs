using System;
using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class Unit : MonoBehaviour, IDamagable
{
    protected HealthSystem _healthSystem;
    public int healthTest;
 
    [SerializeField] private int _maxHealth = 100;

    public event Action OnUnitDestroyed;
    public event Action<int> OnUnitHealthChanged;

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
         OnUnitHealthChanged?.Invoke(_healthSystem.CurrentHealth);
    }

    public virtual void HealthIncrease(int health)
    {
        OnUnitHealthChanged?.Invoke(_healthSystem.IncreaseHealth(health));
    }

    public virtual int GetHealth()
    {
        return _healthSystem.CurrentHealth;
    }

    protected virtual void UnitDestroyed()
    {
        OnUnitDestroyed?.Invoke();
        Destroy(gameObject);
    }

}
