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

    protected EventManager _eventManager;
    [SerializeField] protected UnitType _unitType;
    [SerializeField] protected int _maxHealth = 100;
    [SerializeField] protected int __impactDamage = 10;
    [SerializeField] string _particleEffectTag;
    [SerializeField] string _soundEffectTag;
    [SerializeField] string _collisionIgnore;

    public UnitType UnitType {  get { return _unitType; } }

    public EventManager EventManager { get { return _eventManager; } }

    protected virtual void Awake()
    {
        _eventManager = EventManager.Instance;
    }

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
        _eventManager.OnUnitDestroyed?.Invoke(UnitType,transform.position);
        _eventManager.OnPlaySoundEffect?.Invoke("ShipEffect", transform.position);
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
    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
       
        Vector3 hitPoint = collision.contacts[0].point;
        if (!collision.collider.gameObject.CompareTag(_collisionIgnore)) 
        {
            collision.collider?.attachedRigidbody?.GetComponent<IDamagable>()?.DamageTaken(__impactDamage);
        }

        _eventManager.OnPlayParticleEffect?.Invoke(_particleEffectTag, (Vector2)hitPoint, 1f);
        _eventManager.OnPlaySoundEffect?.Invoke(_soundEffectTag, (Vector2)hitPoint);
    }

}
