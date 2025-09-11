using System;
using UnityEngine;

[RequireComponent(typeof(IMovementController))]
public class Projectile : MonoBehaviour
{
    [SerializeField] private float _lifetime = 1;

    private Action _onLifetimeEnd;

    private static int _counter;
    
    public void Setup(Action onLifetimeEnd) => _onLifetimeEnd = onLifetimeEnd;
    private void OnEnable()
    {
        _counter++;
        gameObject.name = "Projectile " + _counter;
        Invoke(nameof(OnLifetimeEnd), _lifetime);
    }

    private void OnLifetimeEnd() => _onLifetimeEnd?.Invoke();

    private void OnTriggerEnter2D(Collider2D other)
    {
        CancelInvoke();
        OnLifetimeEnd();
    }
}
