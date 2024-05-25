using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    [SerializeField] private Unit[] enemyUnits;
    [SerializeField] private float _spawnDistance = 8f;
    [SerializeField] private float _spawnDelay = 5f;

    [SerializeField] PickupSpawner _pickUpSpawner;

    [SerializeField] EventManager _eventManager;

    private List<GameObject> _enemyUnits;
  //  private List<GameObject> _pickUps;


    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        

    }

    private void Start()
    {
        StartCoroutine(SpawnEnemy());
        _enemyUnits = new List<GameObject> ();
       // _pickUps = new List<GameObject> (); 
    }

    private void OnEnable()
    {
        _eventManager.OnUnitDestroyed += UnitDestroyed;    
    }

    private void OnDisable()
    {
        _eventManager.OnUnitDestroyed -= UnitDestroyed;
    }

    public void UnitDestroyed(UnitType unit, Vector3 position)
    {
        if (unit != UnitType.Player)
        {
            
        }
    }
    public void DestoyAllEnemyUnits()
    {
        Debug.Log("In Destroy all enemy units");
        
        if(_enemyUnits.Count > 0 && _enemyUnits != null)
        {
            foreach (GameObject unit in _enemyUnits)
            {
                if (unit != null)
                {
                    Destroy(unit.gameObject);
                }
            }
            _enemyUnits.Clear();
        }
    }


    IEnumerator SpawnEnemy()
    {
        while (true)
        {
            yield return new WaitForSeconds(_spawnDelay);

            Vector2 randomPosition = Random.insideUnitCircle.normalized * _spawnDistance;

            Vector2 spawnPoint = new Vector2(transform.position.x, transform.position.y) + randomPosition;

            int enemyIndex = Random.Range(0, enemyUnits.Length);

            Unit newShip = Instantiate(enemyUnits[enemyIndex], spawnPoint, Quaternion.identity);
           _enemyUnits.Add(newShip.gameObject);
        }
    }
}
