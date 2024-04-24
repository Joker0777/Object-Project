using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool : MonoBehaviour
{
    [SerializeField] private int PoolSize;

    //public Asteroid asteroidPrefab;

   // public ObjectPool<Asteroid> asteroidPool;

    private Transform spawner;

    private string poolTag = "Asteroid";


    private void Awake()
    {
        spawner = transform.root.GetComponent<Transform>();
    }
    void Start()
    {
        //asteroidPool = new ObjectPool<Asteroid> (asteroidPrefab,asteroidPoolSize,spawner, poolTag);
    }


   // public Asteroid GetAsteroid()
   // {
      //  Asteroid asteroid = asteroidPool.GetObject();
      //  return asteroid;
   // }
}
