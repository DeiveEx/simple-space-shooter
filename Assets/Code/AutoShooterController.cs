using System;
using UnityEngine;

[RequireComponent(typeof(IGunController))]
public class AutoShooterController : MonoBehaviour
{
    [SerializeField] private Transform _player;
    [SerializeField] private Transform _gunHolder;
    [SerializeField] private float _dangerRange = 5;
    [SerializeField] private float _greedRange = 5;

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

    private void OnValidate()
    {
        if (_greedRange < _dangerRange)
        {
            Debug.LogWarning($"{nameof(_greedRange)} cannot be lower than {nameof(_dangerRange)}");
            _greedRange = _dangerRange;
        }
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

            if (distance > _dangerRange || distance > closestDistance)
                continue;
            
            closest = asteroid;
            closestDistance = distance;
        }
        
        if(closest == null)
            return;
        
        ShootAtAsteroid(closest);
    }

    private void TryShootMostValuableAsteroid()
    {
        if(!_gun.CanShoot)
            return;

        AsteroidController mostValuable = null;
        float targetDistance = float.MaxValue;
        int highestPoints = 0;
        
        foreach (var asteroid in AsteroidManager.Asteroids)
        {
            var distance = Vector2.Distance(_player.position, asteroid.transform.position);

            if (distance > _greedRange)
                continue;

            var points = 0; //TODO
            
            if(points < highestPoints)
                continue;

            //Check if the asteroid gives more points or, if it gives the same amount of points, if it's closer
            if (points > highestPoints || distance < targetDistance)
            {
                mostValuable = asteroid;
                targetDistance = distance;
            }
        }
        
        if(mostValuable == null)
            return;
        
        ShootAtAsteroid(mostValuable);
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
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _dangerRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position,_greedRange);
    }
}
