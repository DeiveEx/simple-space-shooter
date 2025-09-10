using UnityEngine;
using Random = UnityEngine.Random;

public class AsteroidSplitOnDamage : MonoBehaviour, IDamageable
{
    //TODO move this data to a Scriptable Object?
    [SerializeField] private GameObject _splitAsteroidPrefab;
    [SerializeField] private int _splitAmount = 2;
    [SerializeField] private float _splitAngleOffset = 30f;
    [SerializeField] private float _splitSpeedMultiplier = 1.5f;

    private Rigidbody2D _rigidbody;
    
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    public void Damage(int amount)
    {
        //Create the split pieces
        for (int i = 0; i < _splitAmount; i++)
        {
            var child = Instantiate(_splitAsteroidPrefab, transform.position, Quaternion.identity);
            var childRb = child.GetComponent<Rigidbody2D>();

            //Randomly rotate the pieces a bit based on the angle offset
            var velocity = _rigidbody.linearVelocity;
            
            velocity = Quaternion.Euler(0, 0, Random.Range(-_splitAngleOffset, _splitAngleOffset)) * velocity;
            childRb.linearVelocity = velocity * _splitSpeedMultiplier;
        }
        
        Destroy(gameObject);
    }
}
