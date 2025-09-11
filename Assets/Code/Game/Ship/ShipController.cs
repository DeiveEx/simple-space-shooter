using UnityEngine;

[RequireComponent(typeof(IMovementController))]
[RequireComponent(typeof(IGunController))]
[RequireComponent(typeof(HealthComponent))]
public class ShipController : MonoBehaviour
{
    private IMovementController _movement;
    private IGunController _gun;
    private HealthComponent _health;

    public IMovementController Movement => _movement;
    public IGunController Gun => _gun;
    public HealthComponent Health => _health;
    private GameSettings GameSettings => SimpleServiceLocator.GetService<GameSettings>();

    private void Awake()
    {
        _movement = GetComponent<IMovementController>();
        _gun = GetComponent<IGunController>();
        _health = GetComponent<HealthComponent>();

        _health.Died += OnDied;
    }

    public void Setup()
    {
        _movement.Setup(GameSettings.ShipSpeed);
        _gun.Setup(GameSettings.GunFireRate);
    }

    private void OnDied()
    {
        Destroy(gameObject);
    }
}
