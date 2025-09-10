using UnityEngine;

public class HealthComponent : MonoBehaviour, IDamageable
{
    [SerializeField] private int _health = 3;
    
    public void Damage(int amount)
    {
        _health -= amount;

        if (_health <= 0)
            Die();
    }

    private void Die()
    {
        Debug.Log("Died");
        Destroy(gameObject);
    }
}
