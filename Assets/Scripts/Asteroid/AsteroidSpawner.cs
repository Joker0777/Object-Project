using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class AsteroidSpawner : MonoBehaviour
{
    [SerializeField] float spawnRate = 2.0f;
    [SerializeField] int spawnCount = 1;
    [SerializeField] float spawnDistance = 15f;
    [SerializeField] float spawnAngle = 15f;

    private EventManager _eventManager;

    [SerializeField] GameObject _explosionPrefab;
    private ParticleSystem[] _asteroidDestroyedEffect;
    private AsteroidPool _asteroidPool;

    private void Awake()
    {
        _asteroidDestroyedEffect = GetComponentsInChildren<ParticleSystem>();
        _asteroidPool = GetComponentInChildren<AsteroidPool>();
        _eventManager = EventManager.Instance;
    }
    void Start()
    {      
        InvokeRepeating(nameof(Spawn),this.spawnRate,this.spawnRate);
    }

    private void OnEnable()
    {
        _eventManager.OnAstreroidSplitEvent += BreakAsteroid;
    }

    private void OnDisable() 
    { 
        _eventManager.OnAstreroidSplitEvent -= BreakAsteroid;
    }
    private void Spawn()
    {
  
        for (int i = 0; i < spawnCount; i++)
        {
            Vector2 spawnDirection = Random.insideUnitCircle.normalized * spawnDistance;
            Vector2 spawnPoint = (Vector2)transform.position + spawnDirection;      
            Quaternion rotation = Quaternion.AngleAxis(Random.Range(-spawnAngle, spawnAngle), Vector3.forward);

            Asteroid asteroid = _asteroidPool.GetMainAsteroid();

            if (asteroid != null)
            {
                SetUpAsteroid(spawnDirection, spawnPoint, rotation, asteroid);
            }
        }
    }

    private static void SetUpAsteroid(Vector2 spawnDirection, Vector2 spawnPoint, Quaternion rotation, Asteroid asteroid)
    {
        asteroid.transform.rotation = rotation;
        asteroid.transform.position = spawnPoint;

        asteroid.AsteroidSize = Random.Range(asteroid.AsteroidMinSize, asteroid.AsteroidMaxSize);
        asteroid.transform.localScale = Vector3.one * asteroid.AsteroidSize;

        asteroid.ResetHealth(Mathf.FloorToInt(Mathf.Lerp(5, 15, ((asteroid.AsteroidSize - asteroid.AsteroidMinSize) / (asteroid.AsteroidSize - asteroid.AsteroidMinSize)))));

        asteroid.SetTrajectory(rotation * -spawnDirection);
    }


    private void BreakAsteroid(float size, Asteroid mainAsteroid)
    {
        Asteroid[] brokenAsteroids = _asteroidPool.GetBrokenAsteroid(mainAsteroid);

        if (brokenAsteroids != null)
        {
            foreach (Asteroid brokenAsteroid in brokenAsteroids)
            {
                brokenAsteroid.transform.localScale = Vector3.one * mainAsteroid.AsteroidSize;
                brokenAsteroid.AsteroidSize = size * .5f;
                brokenAsteroid.transform.position = mainAsteroid.transform.position;
                brokenAsteroid.transform.rotation = mainAsteroid.transform.rotation;
                brokenAsteroid.SetTrajectory(Random.insideUnitCircle.normalized);
            }
        }     
    } 
}
