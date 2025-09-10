using System;
using UnityEngine;

[RequireComponent(typeof(IMovementController))]
public class Projectile : MonoBehaviour
{
    [SerializeField] private float _lifetime = 1;

    private Action _onLifetimeEnd;
    
    public void Setup(Action onLifetimeEnd)
    {
        _onLifetimeEnd = onLifetimeEnd;
    }
    
    private void OnEnable()
    {
        Invoke(nameof(OnLifetimeEnd), _lifetime);
    }

    private void OnLifetimeEnd()
    {
        CancelInvoke();
        _onLifetimeEnd?.Invoke();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        OnLifetimeEnd();
    }
}
