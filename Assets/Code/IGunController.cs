public interface IGunController
{
    public bool CanShoot { get; }
    public Projectile ProjectilePrefab { get; }
    
    void Shoot();
}