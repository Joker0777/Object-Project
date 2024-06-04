using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
  //  [SerializeField] private Unit[] enemyUnits;

  //  [SerializeField] private float _spawnDelay = 5f;

  //  [SerializeField] PickupSpawner _pickUpSpawner;

    [SerializeField] EventManager _eventManager;

  //  private List<GameObject> _enemyUnits;


    [SerializeField] private List<EnemyWave> _enemyWaves;
    [SerializeField] private float _nextWaveDelay;
    [SerializeField] private float _spawnDistance = 8f;

    private int _totalWaves;
    private int _currentWave;
    private List<GameObject> _currentWaveUnits = new List<GameObject>();


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
        StartCoroutine(StartNextWave());
 
        _totalWaves = _enemyWaves.Count;

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

    private void SpawnEnemy(Unit unit)
    {
        Debug.Log("In spawn enemy");

            Vector2 randomPosition = Random.insideUnitCircle.normalized * _spawnDistance;

            Vector2 spawnPoint = new Vector2(transform.position.x, transform.position.y) + randomPosition;

          //  int enemyIndex = Random.Range(0, enemyUnits.Length);

            Unit newShip = Instantiate(unit, spawnPoint, Quaternion.identity);
        _currentWaveUnits.Add(newShip.gameObject);

        //  _enemyUnits.Add(newShip.gameObject);       
    }
    private IEnumerator StartNextWave()
    {
        Debug.Log("In start next wave " + _currentWave);
        
        while (_currentWave < _enemyWaves.Count)
        {
            yield return StartCoroutine(SpawnEnemyWave(_enemyWaves[_currentWave]));
            _currentWave++;
            yield return new WaitForSeconds(_nextWaveDelay);
        }     
    }

    IEnumerator SpawnEnemyWave(EnemyWave wave)
    {
        Debug.Log("In spawn enemy wave");
        
       List<Unit> newWave = wave.GetEnemyUnits();
        _eventManager.OnUIChange.Invoke(UIElementType.WaveEnemies, newWave.Count.ToString());

        while (newWave.Count > 0) 
        { 
            Unit nextUnit = newWave[0];
            if (nextUnit != null)
            {
                SpawnEnemy(nextUnit);
               // yield return new WaitForSeconds(wave.SpawnInterval);
            }
            newWave.RemoveAt(0);
        }

        while(!CurrentWaveIsDestroyed())
        {
            yield return null;
        }

    }

    private bool CurrentWaveIsDestroyed()
    {

        _currentWaveUnits.RemoveAll(unit => unit == null);
        _eventManager.OnUIChange.Invoke(UIElementType.WaveEnemies, _currentWaveUnits.Count.ToString());
        return _currentWaveUnits.Count == 0;
    }
}
