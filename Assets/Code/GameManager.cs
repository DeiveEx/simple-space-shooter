using System.Collections;
using UnityEngine;

[DefaultExecutionOrder(-999)]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    [SerializeField] private AsteroidManager _asteroidManager;
    [SerializeField] private PlayerController _player;
    [SerializeField] private float _respawnDelay = 1;
    [SerializeField] private int _initialAsteroidAmount = 5;
    [SerializeField] private AnimationCurve _difficultyCurve = AnimationCurve.Linear(0, 0, 1, 1);

    private IEventBus _eventBus;
    private int _currentLevel;
    
    public AsteroidManager AsteroidManager => _asteroidManager;
    public PlayerController Player => _player;
    public IEventBus EventBus => _eventBus;

    private void Awake()
    {
        Instance = this;
        _eventBus = new SimpleEventBus();
        _player.Health.HealthChanged += OnPlayerHealthChanged;
        _player.Health.Died += GameOver;
        
        //Since the GameManager owns the EventBus, we don't need to unregister
        EventBus.RegisterHandler<AsteroidsClearedEvent>(OnAsteroidsDestroyed);
    }

    private IEnumerator Start()
    {
        _player.SpawnShip();
        
        //Wait a frame so other classes can execute their start methods
        yield return null;
        
        StartGame();
    }

    private void StartGame()
    {
        AsteroidManager.SpawnRandomAsteroids(_initialAsteroidAmount);
        
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
        yield return new WaitForSeconds(_respawnDelay);
        _player.SpawnShip();
    }
    
    private void OnAsteroidsDestroyed(AsteroidsClearedEvent obj)
    {
        _currentLevel++;
        int levelAmount = Mathf.FloorToInt(_difficultyCurve.Evaluate(_currentLevel));
        AsteroidManager.SpawnRandomAsteroids(_initialAsteroidAmount + levelAmount);
    }
}
