using UnityEngine;

public class HealthComponent : MonoBehaviour, IDamageable
{
    [SerializeField] private int _health = 3;
    
    public void Damage()
    {
        _health--;

        if (_health <= 0)
            Die();
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
