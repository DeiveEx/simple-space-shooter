using UnityEngine;
using UnityEngine.Pool;

public class GunController : MonoBehaviour, IGunController
{
    [SerializeField] private Projectile _bulletPrefab;
    [SerializeField] private Transform _gunBarrel;
    [SerializeField] private float _fireRate = 1;

    private ObjectPool<Projectile> _projectilePool;
    private float _lastShootTime;

    private void Awake()
    {
        _projectilePool = new ObjectPool<Projectile>(
            () =>
            {
                var bullet = Instantiate(_bulletPrefab);
                bullet.Setup(() => _projectilePool.Release(bullet));
                return bullet;
            },
            p =>
            {
                p.gameObject.SetActive(true);
                p.transform.SetPositionAndRotation(_gunBarrel.position, _gunBarrel.rotation);
                p.transform.SetParent(null);
            },
            p =>
            {
                p.transform.SetParent(transform);
                p.gameObject.SetActive(false);
            });
    }

    public void Shoot()
    {
        if(Time.time - _lastShootTime < 1f / _fireRate)
            return;

        _projectilePool.Get();
        _lastShootTime = Time.time;
    }
}
