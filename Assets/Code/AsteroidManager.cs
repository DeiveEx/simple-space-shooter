using UnityEngine;

public class AsteroidManager : MonoBehaviour
{
    [SerializeField] private GameObject _asteroidPrefab;
    [SerializeField] private int _initialAmount = 5;
    [SerializeField] private float _initialSpeed = 5;
    [SerializeField] private Transform _player;
    [SerializeField] private float _minPlayerRange = 5;

    private Camera _camera;
    
    private void Awake()
    {
        _camera = Camera.main;
        
        for (int i = 0; i < _initialAmount; i++)
        {
            SpawnAsteroid();
        }
    }

    private void SpawnAsteroid()
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
        while(Vector2.Distance(_player.position, spawnPos) < _minPlayerRange);
        
        var asteroid = Instantiate(_asteroidPrefab, spawnPos, Quaternion.identity);
        
        //Add a force in a random direction
        var rb = asteroid.GetComponent<Rigidbody2D>();
        var randomDirection = Random.insideUnitCircle.normalized;
        
        rb.AddForce(randomDirection * _initialSpeed, ForceMode2D.Impulse);
    }
}
