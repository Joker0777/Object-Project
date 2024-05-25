using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AsteroidPrefab
{
    [SerializeField] private Asteroid _mainAsteroidPrefab;
    [SerializeField] private Asteroid[] _brokenAsteroidPrefabs;

    public Asteroid MainAsteroidPrefab { get { return _mainAsteroidPrefab; } }
    public Asteroid[] BrokenAsteroidPrefabs { get { return _brokenAsteroidPrefabs;} }

}

