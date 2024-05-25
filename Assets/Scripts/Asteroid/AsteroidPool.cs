using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidPool : MonoBehaviour
{
    [SerializeField] private int asteroidPoolSize;

    [SerializeField] private AsteroidPrefab[] _asteroidPrefabs;

    private List<ObjectPool<Asteroid>> mainAsteroidPools;
    private Dictionary<string, ObjectPool<Asteroid>[]> brokenAsteroidPools;

    private Transform asteroidSpawner;

    private string mainAsteroidPoolTag = "MainAsteroid";
    private string brokenAsteroidPoolTag = "BrokenAsteroid";

    private void Awake()
    {
        asteroidSpawner = transform.root.GetComponent<Transform>();
        mainAsteroidPools = new List<ObjectPool<Asteroid>>();
        brokenAsteroidPools = new Dictionary<string, ObjectPool<Asteroid>[]>();
    }
    void Start()
    {
        InitializeMainPools(mainAsteroidPoolTag);
        InitializeBrokenPools(brokenAsteroidPoolTag);
    }

    public Asteroid GetMainAsteroid()
    {
        return GetRandomAsteroidFromPool(mainAsteroidPools);
    }

    public Asteroid[] GetBrokenAsteroid(Asteroid mainAsteroid)
    {
        Asteroid[] asteroids = null;

        if (brokenAsteroidPools.TryGetValue(mainAsteroid.AsteroidID, out ObjectPool<Asteroid>[] value))
        {
            asteroids = new Asteroid[value.Length];

            for (int i = 0; i < asteroids.Length; i++)
            {
                asteroids[i] = value[i].GetObject();
            }
        }
        return asteroids;
    }

    private Asteroid GetRandomAsteroidFromPool(List<ObjectPool<Asteroid>> pools)
    {
        if (pools.Count == 0) return null;
        return pools[Random.Range(0, pools.Count)].GetObject();
    }

    private void InitializeMainPools(string poolTag)
    {
        for (int i = 0; i < _asteroidPrefabs.Length; i++)
        {
            mainAsteroidPools.Add(new ObjectPool<Asteroid>(_asteroidPrefabs[i].MainAsteroidPrefab, asteroidPoolSize, asteroidSpawner, $"{poolTag}{i}"));
        }
    }

    private void InitializeBrokenPools(string poolTag)
    {
        for (int i = 0; i < _asteroidPrefabs.Length; i++)
        {
            if (_asteroidPrefabs[i].BrokenAsteroidPrefabs.Length > 0)
            {
                ObjectPool<Asteroid>[] currentBrokenPools = new ObjectPool<Asteroid>[_asteroidPrefabs[i].BrokenAsteroidPrefabs.Length];

                for (int j = 0; j < _asteroidPrefabs[i].BrokenAsteroidPrefabs.Length; j++)
                {
                    currentBrokenPools[j] = new ObjectPool<Asteroid>(_asteroidPrefabs[i].BrokenAsteroidPrefabs[j], asteroidPoolSize, asteroidSpawner, $"{poolTag}{i}");
                }

                brokenAsteroidPools.Add(_asteroidPrefabs[i].MainAsteroidPrefab.AsteroidID, currentBrokenPools);
            }

        }
    }
}
