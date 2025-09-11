using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    [SerializeField] AsteroidManager _asteroidManager;
    [SerializeField] PlayerController _player;

    private IEventBus _eventBus;
    
    public AsteroidManager AsteroidManager => _asteroidManager;
    public PlayerController Player => _player;
    public IEventBus EventBus => _eventBus;

    private void Awake()
    {
        Instance = this;
        _player.SpawnShip();
        _eventBus = new SimpleEventBus();
    }

    private void Start()
    {
        _player.Health.Died += GameOver;
    }

    private void GameOver()
    {
        Debug.Log("Game Over");
    }
}
