using UnityEngine;

[RequireComponent(typeof(IGunController))]
public class AutoShooterController : MonoBehaviour
{
    [SerializeField] private Transform _player;
    [SerializeField] private Transform _gunHolder;
    [SerializeField] private float _shootRange = 5;

    private IGunController _gun;
    private float _bulletSpeed;

    private AsteroidManager AsteroidManager => GameManager.Instance.AsteroidManager;

    private void Awake()
    {
        _gun = GetComponent<IGunController>();
        _bulletSpeed = _gun.ProjectilePrefab.GetComponent<IMovementController>().Speed;
    }

    private void Update()
    {
        TryShootClosestAsteroid();
        TryShootMostValuableAsteroid();
    }

    private void TryShootClosestAsteroid()
    {
        if(!_gun.CanShoot)
            return;

        AsteroidController closest = null;
        float closestDistance = float.MaxValue;
        
        foreach (var asteroid in AsteroidManager.Asteroids)
        {
            var distance = Vector2.Distance(_player.position, asteroid.transform.position);

            if (distance < _shootRange && distance < closestDistance)
            {
                closest = asteroid;
                closestDistance = distance;
            }
        }
        
        if(closest == null)
            return;
        
        ShootAtAsteroid(closest);
    }

    private void TryShootMostValuableAsteroid()
    {
        if(!_gun.CanShoot)
            return;

        //TODO
    }

    private void ShootAtAsteroid(AsteroidController target)
    {
        //Predict the asteroid position based on their velocity. This requires knowing the bullet speed as well
        var asteroidVelocity = (Vector3)target.Rigidbody.linearVelocity;
        var dirToTarget = target.transform.position - _player.position;
        var timeToHit = dirToTarget.magnitude / _bulletSpeed;
        var predictedPosition = target.transform.position + asteroidVelocity * timeToHit;
        var shootDir = predictedPosition - _player.position;
        
        _gunHolder.transform.up = shootDir.normalized;
        _gun.Shoot();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, _shootRange);
    }
}
