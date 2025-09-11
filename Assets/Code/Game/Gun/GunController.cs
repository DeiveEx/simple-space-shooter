using Systems.ServiceLocator;
using UnityEngine;
using UnityEngine.Pool;

public class GunController : MonoBehaviour, IGunController
{
    [SerializeField] private Projectile _bulletPrefab;
    [SerializeField] private Transform _gunBarrel;
    [SerializeField] private float _fireRate = 1;

    private IObjectPool<Projectile> _projectilePool;
    private float _projectileLifetime;
    private float _lastShootTime;
    
    public bool CanShoot => Time.time - _lastShootTime > 1f / _fireRate;
    public Projectile ProjectilePrefab => _bulletPrefab;
    private GameSettings GameSettings => SimpleServiceLocator.GetService<GameSettings>();

    private void Awake()
    {
        _projectilePool = new ObjectPool<Projectile>(
            () =>
            {
                var bullet = Instantiate(_bulletPrefab);
                bullet.Setup(_projectilePool, GameSettings.ProjectileLifetime, GameSettings.ProjectileSpeed);
                return bullet;
            },
            p =>
            {
                p.gameObject.SetActive(true);
                p.transform.SetPositionAndRotation(_gunBarrel.position, _gunBarrel.rotation);
            },
            p =>
            {
                p.gameObject.SetActive(false);
            });
    }

    public void Setup(float fireRate)
    {
        _fireRate = fireRate;
    }

    public void Shoot()
    {
        if(!CanShoot)
            return;

        _projectilePool.Get();
        _lastShootTime = Time.time;
    }
}
