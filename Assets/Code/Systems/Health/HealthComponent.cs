using System;
using UnityEngine;

namespace Systems.Health
{
    public class HealthComponent : MonoBehaviour, IDamageable
    {
        [SerializeField] private bool _autoSetup;
        [SerializeField] private int _initialHealth = 3;

        private int _currentHealth;
        private bool _isDead;

        public int CurrentHealth => _currentHealth;
        public bool IsDead => _isDead;

        public event Action HealthChanged;
        public event Action Died;

        private void OnEnable()
        {
            if (_autoSetup)
                Setup(_initialHealth);
        }

        public void Setup(int health)
        {
            if (health <= 0)
            {
                Debug.LogError("Initial health cannot be lower or equal to 0");
                return;
            }

            _initialHealth = health;
            _currentHealth = _initialHealth;
            _isDead = false;
        }

        public void Damage(int amount)
        {
            if (_isDead || amount <= 0)
                return;

            _currentHealth -= amount;
            
            if (_currentHealth < 0)
                _currentHealth = 0;
            
            HealthChanged?.Invoke();

            if (_currentHealth == 0)
                Die();
        }

        private void Die()
        {
            _isDead = true;
            Died?.Invoke();
        }
    }
}
