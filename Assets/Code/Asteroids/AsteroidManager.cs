using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

public enum AsteroidSize
{
    None,
    Small,
    Medium,
    Large,
}

public class AsteroidManager : MonoBehaviour
{
    [Serializable]
    private class AsteroidSpec
    {
        public AsteroidSize Size;
        public AsteroidController Prefab;
        public float Speed;
        public IObjectPool<AsteroidController> Pool;
    }
    
    [SerializeField] private AsteroidSpec[] _asteroidSpecs;
    [SerializeField] private float _minPlayerRange = 5;

    private Camera _camera;
    private Dictionary<AsteroidSize, AsteroidSpec> _asteroidSpecMap;
    private readonly List<AsteroidController> _asteroids = new ();
    private ShipController _ship;
    
    public IReadOnlyList<AsteroidController> Asteroids => _asteroids;
    private IEventBus EventBus => SimpleServiceLocator.GetService<IEventBus>();

    private void Awake()
    {
        _camera = Camera.main;
        _asteroidSpecMap = _asteroidSpecs.ToDictionary(spec => spec.Size, spec => spec);
        ConfigurePools();
        
        EventBus.RegisterHandler<ShipSpawnedEvent>(OnShipSpawned);
    }

    private void OnDestroy()
    {
        EventBus?.UnregisterHandler<ShipSpawnedEvent>(OnShipSpawned);
    }
    
    private void OnShipSpawned(ShipSpawnedEvent obj)
    {
        _ship = obj.Ship;
    }

    public void SpawnRandomAsteroids(int amount)
    {
        //Spawn some random initial asteroids
        for (int i = 0; i < amount; i++)
        {
            var spawnPos = GetRandomSpawnPosition();
            var randomDir = Random.insideUnitCircle.normalized;
            SpawnAsteroid(AsteroidSize.Large, spawnPos, randomDir);
        }
    }

    private void ConfigurePools()
    {
        foreach (var spec in _asteroidSpecs)
        {
            spec.Pool = new ObjectPool<AsteroidController>(
                () =>
                {
                    var instance = Instantiate(spec.Prefab, transform);
                    var instanceHealth = instance.GetComponent<HealthComponent>();

                    instanceHealth.Died += () => spec.Pool.Release(instance);
                    
                    return instance;
                },
                asteroid =>
                {
                    asteroid.gameObject.SetActive(true);
                    _asteroids.Add(asteroid);
                },
                asteroid =>
                {
                    asteroid.gameObject.SetActive(false);
                    _asteroids.Remove(asteroid);

                    if(_asteroids.Count == 0)
                        EventBus.Publish(new AsteroidsClearedEvent());
                }
            );
        }
    }

    public void SpawnAsteroid(AsteroidSize size, Vector3 spawnPos, Vector2 direction)
    { 
        var spec = _asteroidSpecMap[size];

        var asteroid = spec.Pool.Get();
        asteroid.transform.SetPositionAndRotation(spawnPos, Quaternion.identity);
        asteroid.Rigidbody.AddForce(direction * spec.Speed, ForceMode2D.Impulse);
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
        while(Vector2.Distance(_ship.transform.position, spawnPos) < _minPlayerRange);

        return spawnPos;
    }
}
