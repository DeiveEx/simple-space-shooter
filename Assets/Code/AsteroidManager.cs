using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AsteroidManager : MonoBehaviour
{
    [SerializeField] private AsteroidController _asteroidPrefab;
    [SerializeField] private int _initialAmount = 5;
    [SerializeField] private float _initialForce = 5;
    [SerializeField] private float _minPlayerRange = 5;

    private Camera _camera;
    private readonly List<AsteroidController> _asteroids = new ();
    
    public IReadOnlyList<AsteroidController> Asteroids => _asteroids;
    private GameObject Player => GameManager.Instance.PlayerShip;
    
    private void Awake()
    {
        _camera = Camera.main;
        
        for (int i = 0; i < _initialAmount; i++)
        {
            var spawnPos = GetRandomSpawnPosition();
            var randomDir = Random.insideUnitCircle.normalized * _initialForce;
            SpawnAsteroid(_asteroidPrefab, spawnPos, randomDir);
        }
    }

    public void SpawnAsteroid(AsteroidController asteroidPrefab, Vector3 spawnPos, Vector2 force)
    { 
        //TODO user Object pool for each asteroid type?
        var asteroid = Instantiate(asteroidPrefab, spawnPos, Quaternion.identity);
        
        //Add a force in a random direction
        var rb = asteroid.GetComponent<Rigidbody2D>();
        rb.AddForce(force, ForceMode2D.Impulse);
    }

    private Vector3 GetRandomSpawnPosition()
    {
        //Find a random position on the screen that is not too close to the player
        Vector3 spawnPos;
        
        do
        {
            var screenPosition = new Vector3()
            {
                x = Random.Range(0, Screen.width),
                y = Random.Range(0, Screen.height),
                z = -_camera.transform.position.z,
            };
            
            spawnPos = _camera.ScreenToWorldPoint(screenPosition);
        }
        while(Vector2.Distance(Player.transform.position, spawnPos) < _minPlayerRange);

        return spawnPos;
    }
    
    public void RegisterAsteroid(AsteroidController asteroid) => _asteroids.Add(asteroid);
    public void UnregisterAsteroid(AsteroidController asteroid) => _asteroids.Remove(asteroid);
}
