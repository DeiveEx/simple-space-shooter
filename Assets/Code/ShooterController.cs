using UnityEngine;

[RequireComponent(typeof(IGunController))]
public class ShooterController : MonoBehaviour
{
    [SerializeField] private AsteroidManager _asteroidManager;
    [SerializeField] private Transform _avatar;
}
