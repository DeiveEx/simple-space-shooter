using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    [SerializeField] AsteroidManager _asteroidManager;
    [SerializeField] GameObject _playerShip;

    public AsteroidManager AsteroidManager => _asteroidManager;
    public GameObject PlayerShip => _playerShip;

    private void Awake()
    {
        Instance = this;
    }
}
