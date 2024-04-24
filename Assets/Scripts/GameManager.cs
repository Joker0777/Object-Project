using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    [SerializeField] private Unit[] enemyPrefabs;
    [SerializeField] private float _spawnDistance = 8f;
    [SerializeField] private float _spawnDelay = 5f;

    [SerializeField] private float _asteroidPrefabs;
    [SerializeField] private float _spawnAngle;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        StartCoroutine(SpawnEnemy());
    }



    private void Update()
    {
        
    }

    // Update is called once per frame
    IEnumerator SpawnEnemy()
    {
        while (true)
        {
            yield return new WaitForSeconds(_spawnDelay);

            Vector2 randomPosition = Random.insideUnitCircle.normalized * _spawnDistance;

            Vector2 spawnPoint = new Vector2(transform.position.x, transform.position.y) + randomPosition;

            int prefeabIndex = Random.Range(0, enemyPrefabs.Length - 1);

            Instantiate(enemyPrefabs[prefeabIndex], spawnPoint, Quaternion.identity);
        }
    }
}
