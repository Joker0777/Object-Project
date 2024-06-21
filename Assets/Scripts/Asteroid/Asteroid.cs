using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;
using System;
using Random = UnityEngine.Random;


public class Asteroid : Unit
{
    private Rigidbody2D _rigidbody;

    private Coroutine asteroidDestroyed;

    [SerializeField] private float _asteroidSize = 1.0f;
    [SerializeField] private float _asteroidMinSize = .5f;
    [SerializeField] private float _asteroidMaxSize = 1.5f;
    [SerializeField] private float _asteroidSpeed = 10f;
    [SerializeField] private float _torqueForce = 10f;
    [SerializeField] string _asteroidID;
    [SerializeField] int _asteroidHitDamage;

    [SerializeField] private float _maxLifeTime = 15f;
    [SerializeField] private int _startingHealth = 20;



    public string AsteroidID { get { return _asteroidID; } }
    public float AsteroidSize { get { return _asteroidSize; } set { _asteroidSize = value;  } }
    public float AsteroidMinSize { get {  return _asteroidMinSize; } }
    public float AsteroidMaxSize { get { return _asteroidMaxSize;  } }

    protected override void Awake()
    {
        base.Awake();
        _rigidbody = GetComponent<Rigidbody2D>();
        _healthSystem = new HealthSystem();
    }

    private void OnEnable()
    {
        _healthSystem.IsDestroyed = false;
        ResetHealth(_healthSystem.MaxHealth);

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
        yield return new WaitForSeconds(_maxLifeTime);

        gameObject.SetActive(false);
    }

    public void SetTrajectory(Vector2 direction)
    {
        _rigidbody.AddForce(direction * _asteroidSpeed, ForceMode2D.Impulse);
        _rigidbody.AddTorque(Random.Range(-_torqueForce, _torqueForce));
    }

    public void DestroyAsteroid()
    {
        gameObject.SetActive(false);
    }

    protected override void UnitDestroyed()
    {
        if ((this._asteroidSize * .5f) >= _asteroidMinSize)
        {
          
            _eventManager.OnAstreroidSplitEvent?.Invoke(_asteroidSize, this);
            _eventManager.OnPlayParticleEffect?.Invoke("AsteroidBreak", transform.position, _asteroidSize);
            gameObject.SetActive(false);
        }
        else
        {
            _eventManager.OnPlayParticleEffect?.Invoke("Asteroid", transform.position, _asteroidSize);
        }
        _eventManager.OnPlaySoundEffect?.Invoke("AsteroidEffect", transform.position);
        _eventManager.OnUnitDestroyed?.Invoke(UnitType, transform.position);

        gameObject.SetActive(false);
    }
}
