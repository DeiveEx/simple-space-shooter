using UnityEngine;
using Random = UnityEngine.Random;

public class AsteroidController : MonoBehaviour, IDamageable
{
    //TODO move this data to a Scriptable Object?
    [SerializeField] private AsteroidController _splitAsteroidPrefab;
    [SerializeField] private int _splitAmount = 2;
    [SerializeField] private float _splitAngleOffset = 30f;
    [SerializeField] private float _splitSpeedMultiplier = 1.5f;

    private Rigidbody2D _rigidbody;

    public Rigidbody2D Rigidbody => _rigidbody;
    private AsteroidManager AsteroidManager => GameManager.Instance.AsteroidManager;

    private void Awake() => _rigidbody = GetComponent<Rigidbody2D>();
    private void OnEnable() => AsteroidManager.RegisterAsteroid(this);
    private void OnDisable() => AsteroidManager.UnregisterAsteroid(this);

    public void Damage(int amount)
    {
        Split();
        Destroy(gameObject);
    }

    private void Split()
    {
        if(_splitAsteroidPrefab == null)
            return;
        
        //Create the split pieces
        for (int i = 0; i < _splitAmount; i++)
        {
            //Randomly rotate the pieces a bit based on the angle offset
            var velocity = _rigidbody.linearVelocity;
            
            velocity = Quaternion.Euler(0, 0, Random.Range(-_splitAngleOffset, _splitAngleOffset)) * velocity;
            AsteroidManager.SpawnAsteroid(_splitAsteroidPrefab, transform.position, velocity * _splitSpeedMultiplier);
        }
    }
}
