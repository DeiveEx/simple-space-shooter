using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    [SerializeField] AsteroidManager _asteroidManager;
    [SerializeField] PlayerController _player;

    public AsteroidManager AsteroidManager => _asteroidManager;
    public PlayerController Player => _player;

    private void Awake()
    {
        Instance = this;
        _player.SpawnShip();
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
