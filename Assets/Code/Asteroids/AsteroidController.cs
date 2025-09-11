using UnityEngine;
using Random = UnityEngine.Random;

public class AsteroidController : MonoBehaviour, IDamageable
{
    [SerializeField] private AsteroidSize _splitSize;
    [SerializeField] private int _splitAmount = 2;
    [SerializeField] private float _splitAngleOffset = 30f;
    [SerializeField] private int _score;

    private Rigidbody2D _rigidbody;

    public Rigidbody2D Rigidbody => _rigidbody;
    private AsteroidManager AsteroidManager => SimpleServiceLocator.GetService<AsteroidManager>();
    private IEventBus EventBus => SimpleServiceLocator.GetService<IEventBus>();

    private void Awake() => _rigidbody = GetComponent<Rigidbody2D>();

    public void Damage(int amount)
    {
        Split();
        EventBus.Publish(new AsteroidDestroyedEvent() { Score = _score });
    }

    private void Split()
    {
        if(_splitSize == AsteroidSize.None)
            return;
        
        //Create the split pieces
        for (int i = 0; i < _splitAmount; i++)
        {
            //Randomly rotate the pieces a bit based on the angle offset
            var velocity = _rigidbody.linearVelocity;
            
            velocity = Quaternion.Euler(0, 0, Random.Range(-_splitAngleOffset, _splitAngleOffset)) * velocity;
            AsteroidManager.SpawnAsteroid(_splitSize, transform.position, velocity);
        }
    }
}
