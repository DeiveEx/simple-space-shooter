using UnityEngine;

namespace Systems.Health
{
    public class DamageZone : MonoBehaviour
    {
        [SerializeField] private int _amount = 1;

        private void OnTriggerEnter2D(Collider2D other)
        {
            var target = other.GetComponents<IDamageable>();

            foreach (var damageable in target)
            {
                damageable.Damage(_amount);
            }
        }
    }
}
