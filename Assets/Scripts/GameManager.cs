using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    [SerializeField] private Unit[] enemyUnits;
    [SerializeField] private float _spawnDistance = 8f;
    [SerializeField] private float _spawnDelay = 5f;

    [SerializeField] private float _asteroidPrefabs;
    [SerializeField] private float _spawnAngle;

    [SerializeField] private int _enemyPoolSize = 10;

    private Unit _nextUnit;

    private void Awake()
    {
        Instance = this;

       // foreach (var enemy in enemyUnits)
        //{
        //    ObjectPoolSystem.Instance.AddPool(_enemyPoolSize, enemy.UnitType.ToString(), enemy, this.transform);
      //  }
    }

    private void Start()
    {



        StartCoroutine(SpawnEnemy());
    }



    IEnumerator SpawnEnemy()
    {
        while (true)
        {
            yield return new WaitForSeconds(_spawnDelay);

            Vector2 randomPosition = Random.insideUnitCircle.normalized * _spawnDistance;

            Vector2 spawnPoint = new Vector2(transform.position.x, transform.position.y) + randomPosition;

            int enemyIndex = Random.Range(0, enemyUnits.Length - 1);

           // _nextUnit = ObjectPoolSystem.Instance.GetObject(enemyUnits[enemyIndex].UnitType.ToString());

           // _nextUnit.transform.SetPositionAndRotation(spawnPoint, Quaternion.identity);



            Instantiate(enemyUnits[enemyIndex], spawnPoint, Quaternion.identity);


        }
    }
}
