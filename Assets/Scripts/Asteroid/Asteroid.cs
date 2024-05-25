using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;
using System;
using Random = UnityEngine.Random;


public class Asteroid : MonoBehaviour, IDamagable
{
    private Rigidbody2D _rigidbody;

    private Coroutine asteroidDestroyed;

    //[SerializeField] private float _asteroidSize = 1.0f;

    public float _asteroidSize = 1.0f;

    //[SerializeField] private float _asteroidMinSize = .5f;

    public float _asteroidMinSize = .5f;

    //[SerializeField] private float _asteroidMazSize = 1.5f;

    public float _asteroidMazSize = 1.5f;

    [SerializeField] private float _asteroidSpeed = 10f;

    [SerializeField] private float _torqueForce = 10f;

    public float maxLife;

    private HealthSystem _healthSystem;
    public int _startingHealth = 20;

    [SerializeField] EventManager _eventManager;
    [SerializeField] string _asteroidID;

    public int ResetHealth
    {
        set 
        { 
            _startingHealth = value;
            _healthSystem.CurrentHealth = _startingHealth; 
            _healthSystem.MaxHealth = _startingHealth;
        }
        get { return _startingHealth; }
    }
    public string AsteroidID { get { return _asteroidID; } }

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _healthSystem = new HealthSystem();
    }

    protected virtual void Start()
    {
        _healthSystem.CurrentHealth = _startingHealth;
        _healthSystem.MaxHealth = _startingHealth;
    }

    private void Update()
    {
        if (_healthSystem.IsDestroyed)
        {
            AsteroidDestroyed();
        }
    }

    private void OnEnable()
    {
        _healthSystem.IsDestroyed = false;
        _healthSystem.CurrentHealth = ResetHealth;

        asteroidDestroyed = StartCoroutine(DeactivateAsteroid());
    }

    void OnDisable()
    {
        if (asteroidDestroyed != null)
        {
            StopCoroutine(asteroidDestroyed);
        }

        asteroidDestroyed = null;
    }

    IEnumerator DeactivateAsteroid()
    {
        yield return new WaitForSeconds(maxLife);

        gameObject.SetActive(false);
    }

    public void SetTrajectory(Vector2 direction)
    {
        _rigidbody.AddForce(direction * _asteroidSpeed);
        _rigidbody.AddTorque(Random.Range(-_torqueForce, _torqueForce));
    }

    public void DestroyAsteroid()
    {
        gameObject.SetActive(false);
    }

    public virtual void DamageTaken(int damage)
    {
        _healthSystem.DecreaseHealth(damage);
    }

    protected virtual void AsteroidDestroyed()
    {
        if ((this._asteroidSize * .5f) >= _asteroidMinSize)
        {
          
            _eventManager.OnAstreroidSplitEvent?.Invoke(_asteroidSize, this);
            gameObject.SetActive(false);
        }

        _eventManager.OnAsteroidDestroyedEffectEvent?.Invoke(transform.position, _asteroidSize);
        gameObject.SetActive(false);
    }
}
