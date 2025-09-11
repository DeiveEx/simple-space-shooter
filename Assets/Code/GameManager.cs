using System.Collections;
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
        _eventBus = new SimpleEventBus();
        _player.Health.Died += GameOver;
    }

    private IEnumerator Start()
    {
        _player.SpawnShip();
        
        //Wait a frame so other classes can execute their start methods
        yield return null;
        
        Debug.Log("Game Started");
        EventBus.Publish(new GameStartedEvent());
    }

    private void GameOver()
    {
        Debug.Log("Game Over");
        EventBus.Publish(new GameOverEvent());
    }
}
