using System;
using UnityEngine;

public class HealthComponent : MonoBehaviour, IDamageable
{
    [SerializeField] private int _initialHealth = 3;

    private int _currentHealth;
    
    public int CurrentHealth => _currentHealth;
    public bool IsAlive => _currentHealth > 0;
    
    public event Action HealthChanged;
    public event Action Died;

    private void Awake()
    {
        _currentHealth = _initialHealth;
    }

    public void Damage(int amount)
    {
        _currentHealth -= amount;
        HealthChanged?.Invoke();

        if (_currentHealth <= 0)
            Die();
    }

    private void Die() => Died?.Invoke();
}
