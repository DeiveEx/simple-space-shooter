using System;
using UnityEngine;

public class HealthComponent : MonoBehaviour, IDamageable
{
    [SerializeField] private int _initialHealth = 3;

    private int _currentHealth;
    public int CurrentHealth => _currentHealth;
    
    public event Action HealthChanged;
    public event Action Died;

    private void Awake()
    {
        ResetHealth();
    }

    public void Damage(int amount)
    {
        _currentHealth -= amount;
        HealthChanged?.Invoke();

        if (_currentHealth <= 0)
            Die();
    }

    private void Die()
    {
        Died?.Invoke();
    }

    public void ResetHealth()
    {
        _currentHealth = _initialHealth;
        HealthChanged?.Invoke();
    }
}
