using UnityEngine;

public class DamageZone : MonoBehaviour
{
    [SerializeField] private int _amount = 1;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.TryGetComponent<IDamageable>(out var damageable))
            damageable.Damage(_amount);
    }
}
