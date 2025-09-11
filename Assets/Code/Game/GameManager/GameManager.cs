using System.Collections;
using Systems.ConfigLoader;
using Systems.EventBus;
using Systems.ServiceLocator;
using UnityEngine;

[DefaultExecutionOrder(-999)]
public class GameManager : MonoBehaviour
{
    [SerializeField] private AsteroidManager _asteroidManager;
    [SerializeField] private PlayerController _player;
    [SerializeField] private ShipController _shipPrefab;
    [SerializeField] private AnimationCurve _difficultyCurve = AnimationCurve.Linear(0, 0, 1, 1);

    private int _currentLevel;
    
    private IEventBus EventBus => SimpleServiceLocator.GetService<IEventBus>();
    private GameSettings GameSettings => SimpleServiceLocator.GetService<GameSettings>();

    private void Awake()
    {
        RegisterServices();
        
        _player.Lives.HealthChanged += OnPlayerHealthChanged;
        _player.Lives.Died += GameOver;
        
        //Since the GameManager owns the EventBus, we don't need to unregister
        EventBus.RegisterHandler<AsteroidsClearedEvent>(OnAsteroidsDestroyed);
    }

    private IEnumerator Start()
    {
        _player.Setup();
        
        //Wait a frame so other classes can execute their start methods
        yield return null;
        
        StartGame();
    }

    private void OnDestroy()
    {
        SimpleServiceLocator.DisposeServices();
    }

    private void RegisterServices()
    {
        SimpleServiceLocator.RegisterService<IEventBus>(new SimpleEventBus());
        SimpleServiceLocator.RegisterService(_asteroidManager);
        SimpleServiceLocator.RegisterService(_player);
        
        var configLoader = new JsonLoader(Application.streamingAssetsPath);
        SimpleServiceLocator.RegisterService<IConfigLoader>(configLoader);

        if (!configLoader.TryLoadConfig<GameSettings>("GameSettings", out var gameSettings))
            gameSettings = new();
        
        SimpleServiceLocator.RegisterService(gameSettings);
    }
    
    private void StartGame()
    {
        SpawnShip();
        _asteroidManager.SpawnRandomAsteroids(GameSettings.InitialAsteroidAmount);
        
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
        if(_player.Lives.CurrentHealth <= 0)
            return;

        StartCoroutine(WaitAndSpawnShip());
    }

    private IEnumerator WaitAndSpawnShip()
    {
        yield return new WaitForSeconds(GameSettings.RespawnDelay);
        SpawnShip();
    }
    
    private void SpawnShip()
    {
        var ship = Instantiate(_shipPrefab, Vector3.zero, Quaternion.identity);
        ship.Setup();
        _player.PossessShip(ship);

        EventBus.Publish(new ShipSpawnedEvent() { Ship = ship });
    }
    
    private void OnAsteroidsDestroyed(AsteroidsClearedEvent obj)
    {
        _currentLevel++;
        int levelAmount = Mathf.FloorToInt(_difficultyCurve.Evaluate(_currentLevel));
        _asteroidManager.SpawnRandomAsteroids(GameSettings.InitialAsteroidAmount + levelAmount);
    }
}
