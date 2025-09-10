using UnityEngine;

public class DamageZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"Collided with {other.name}");
        if(other.TryGetComponent<IDamageable>(out var damageable))
            damageable.Damage();
    }
}
