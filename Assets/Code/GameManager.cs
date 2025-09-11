using System.Collections;
using UnityEngine;

[DefaultExecutionOrder(-999)]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    [SerializeField] private AsteroidManager _asteroidManager;
    [SerializeField] private PlayerController _player;
    [SerializeField] private GameSettings _gameSettings;
    [SerializeField] private AnimationCurve _difficultyCurve = AnimationCurve.Linear(0, 0, 1, 1);

    private IEventBus _eventBus;
    private IConfigLoader _configLoader;
    private int _currentLevel;
    
    public AsteroidManager AsteroidManager => _asteroidManager;
    public PlayerController Player => _player;
    public IEventBus EventBus => _eventBus;
    public GameSettings GameSettings => _gameSettings;

    private void Awake()
    {
        Instance = this;
        _eventBus = new SimpleEventBus();
        _configLoader = new JsonLoader(Application.streamingAssetsPath);
        
        _player.Health.HealthChanged += OnPlayerHealthChanged;
        _player.Health.Died += GameOver;
        
        //Since the GameManager owns the EventBus, we don't need to unregister
        EventBus.RegisterHandler<AsteroidsClearedEvent>(OnAsteroidsDestroyed);
        
        if(!_configLoader.TryLoadConfig("GameSettings", out _gameSettings))
            _gameSettings = new GameSettings();
    }

    private IEnumerator Start()
    {
        _player.Setup();
        // _asteroidManager.Setup();
        
        //Wait a frame so other classes can execute their start methods
        yield return null;
        
        StartGame();
    }

    private void StartGame()
    {
        _player.SpawnShip();
        AsteroidManager.SpawnRandomAsteroids(_gameSettings.InitialAsteroidAmount);
        
        Debug.Log("Game Started");
        EventBus.Publish(new GameStartedEvent());
    }

    private void GameOver()
    {
        Debug.Log("Game Over");
        EventBus.Publish(new GameOverEvent());
    }
    
    private void OnPlayerHealthChanged()
    {
        if(_player.Health.CurrentHealth <= 0)
            return;

        StartCoroutine(WaitAndSpawnShip());
    }

    private IEnumerator WaitAndSpawnShip()
    {
        yield return new WaitForSeconds(_gameSettings.RespawnDelay);
        _player.SpawnShip();
    }
    
    private void OnAsteroidsDestroyed(AsteroidsClearedEvent obj)
    {
        _currentLevel++;
        int levelAmount = Mathf.FloorToInt(_difficultyCurve.Evaluate(_currentLevel));
        AsteroidManager.SpawnRandomAsteroids(_gameSettings.InitialAsteroidAmount + levelAmount);
    }
}
