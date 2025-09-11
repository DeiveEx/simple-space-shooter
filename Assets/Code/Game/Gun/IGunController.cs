public interface IGunController
{
    public bool CanShoot { get; }
    public Projectile ProjectilePrefab { get; }
    
    void Setup(float fireRate);
    void Shoot();
}