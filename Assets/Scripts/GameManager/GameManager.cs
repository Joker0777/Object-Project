using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    private EventManager _eventManager;
   // [SerializeField] ScoreManager _scoreManager;
    [SerializeField] Unit _player;

    [SerializeField] private GameObject _missionCompleteScreen;
    [SerializeField] private GameObject _missionFailScreen;
    [SerializeField] private GameObject _nextWaveWarning;
    [SerializeField] private UIPause _pauseMenu;


    [SerializeField] private List<EnemyWave> _enemyWaves;
    [SerializeField] private float _nextWaveDelay;
    [SerializeField] private float _spawnDistance = 8f;
    [SerializeField] private int _lives = 3;
    [SerializeField] private float _respawnAvoidCollisonTime = 3f;
    [SerializeField] private float _respawnTime = 2f;

    private Timer _invulnerableTimer;
    private int _totalWaves;
    private int _currentWave;
    private List<GameObject> _currentWaveUnits = new List<GameObject>();
    private Unit _currentPlayer;
    private bool _isPaused;

    public int Lives 
    {  
        get 
        { 
            return _lives; 
        } 
        set 
        {
            _lives = value;
            _eventManager.OnUIChange?.Invoke(UIElementType.Lives,Lives.ToString());
        }
    }


    private void Awake()
    {
       _eventManager = EventManager.Instance;
        if(Instance == null)
        {
            Instance = this;
           // DontDestroyOnLoad(gameObject);
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

        _invulnerableTimer = new Timer(_respawnAvoidCollisonTime);
        _eventManager.OnUIChange?.Invoke(UIElementType.InvulnerableCountdown, "0");
        _eventManager.OnUIChange?.Invoke(UIElementType.Lives, Lives.ToString());

        Time.timeScale = 1f;

        SpawnPlayer( transform.position);
        
    }

    private void OnEnable()
    {
        _eventManager.OnUnitDestroyed += UnitDestroyed;
        _eventManager.OnPauseGame += PauseGame;

    }

    private void OnDisable()
    {
        _eventManager.OnUnitDestroyed -= UnitDestroyed;
        _eventManager.OnPauseGame -= PauseGame;
    }
    private void Update()
    {
        if (_invulnerableTimer.IsRunningBasic())
        {
            _invulnerableTimer.UpdateTimerBasic(Time.deltaTime);

            _eventManager.OnUIChange?.Invoke(UIElementType.InvulnerableCountdown, Mathf.CeilToInt(_invulnerableTimer.TimeRemaining).ToString());
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _eventManager.OnPauseGame.Invoke();
        }
    }

    public void PauseGame()
    {
        if (!_isPaused)
        {
            _pauseMenu.gameObject.SetActive(true);
            _currentPlayer.gameObject.SetActive(false);
            Time.timeScale = 0f;
            _isPaused = true;
        }
        else
        {
            ResumeGame();
        }

    }

    private void ResumeGame()
    {
        _pauseMenu.gameObject.SetActive(false);
        _currentPlayer.gameObject.SetActive(true);
        Time.timeScale = 1f;
        _isPaused = false;
    }

    public void UnitDestroyed(UnitType unit, Vector3 position)
    {
        if (unit == UnitType.Player)
        {

            Lives--;
            if (Lives == 0)
            {
                GameOver();
            }
            else
            {
                StartCoroutine(RespawnWithDelay(position, _respawnTime));
                Debug.Log("After coroutine start");
            }
        }
        else
        {
            _eventManager.OnScoreIncrease?.Invoke(unit.ToString());
            Debug.Log(unit.ToString());
        }
    }

    private void SpawnEnemy(Unit unit)
    {

            Vector2 randomPosition = Random.insideUnitCircle.normalized * _spawnDistance;

            Vector2 spawnPoint = new Vector2(transform.position.x, transform.position.y) + randomPosition;

            Unit newShip = Instantiate(unit, spawnPoint, Quaternion.identity);
            _currentWaveUnits.Add(newShip.gameObject);    
    }
    private IEnumerator StartNextWave()
    {      
        while (_currentWave < _enemyWaves.Count)
        {
            yield return StartCoroutine(SpawnEnemyWave(_enemyWaves[_currentWave]));
            _currentWave++;
            _nextWaveWarning.SetActive(true);
            yield return new WaitForSeconds(_nextWaveDelay);
            _nextWaveWarning.SetActive(false);
        }
        _missionCompleteScreen.SetActive(true);
        _player.gameObject.SetActive(false);
        _eventManager.OnGameSceneEnd?.Invoke();
    }

    IEnumerator SpawnEnemyWave(EnemyWave wave)
    {      
       List<Unit> newWave = wave.GetEnemyUnits();
        _eventManager.OnUIChange.Invoke(UIElementType.WaveEnemies, newWave.Count.ToString());

        while (newWave.Count > 0) 
        { 
            Unit nextUnit = newWave[0];
            if (nextUnit != null)
            {
                SpawnEnemy(nextUnit);
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
  
    private void SpawnPlayer(Vector3 pos)
    {
        if (Lives > 0)
        {
            _currentPlayer = Instantiate(_player, pos, Quaternion.identity);
            Collider2D collider2D = _currentPlayer.GetComponent<Collider2D>();
            Debug.Log(_currentPlayer);
            collider2D.enabled = false;
            StartCoroutine(AvoidCollison(collider2D));
            _invulnerableTimer.StartTimerBasic();
            _eventManager.OnPlayerRespawn?.Invoke(_currentPlayer);
        }

    }
    private void GameOver()
    {
        _missionFailScreen.SetActive(true);
        _eventManager.OnGameSceneEnd?.Invoke();
    }

    private IEnumerator AvoidCollison(Collider2D playerCollider)
    {
        yield return new WaitForSeconds(_respawnAvoidCollisonTime);
        playerCollider.enabled = true;
        _eventManager.OnUIChange?.Invoke(UIElementType.InvulnerableCountdown, "0");
    }
    IEnumerator RespawnWithDelay(Vector3 pos, float delay)
    {
        yield return new WaitForSeconds(delay);
        SpawnPlayer(pos);
    }
}
