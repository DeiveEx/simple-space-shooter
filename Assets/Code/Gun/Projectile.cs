using UnityEngine;
using UnityEngine.Pool;

[RequireComponent(typeof(IMovementController))]
public class Projectile : MonoBehaviour
{
    [SerializeField] private float _lifetime = 1;

    private IObjectPool<Projectile> _pool;
    private IMovementController _movement;

    private void Awake()
    {
        _movement = GetComponent<IMovementController>();
    }

    public void Setup(IObjectPool<Projectile> pool, float lifetime, float speed)
    {
        _pool = pool;
        _lifetime = lifetime;
        _movement.Setup(speed);
        
        Invoke(nameof(OnLifetimeEnd), _lifetime);
    }

    private void OnLifetimeEnd()
    {
        if(!gameObject.activeSelf)
            return;
        
        _pool.Release(this);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        CancelInvoke();
        OnLifetimeEnd();
    }
}
