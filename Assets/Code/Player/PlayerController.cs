using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private ShipController _shipPrefab;
    [SerializeField] private InputHandler _inputHandler;
    [SerializeField] private HealthComponent _health;
    [SerializeField] private ScoreComponent _score;
    
    private ShipController _shipController;

    public ShipController Ship => _shipController;
    public HealthComponent Health => _health;
    public ScoreComponent Score => _score;
    private IEventBus EventBus => GameManager.Instance.EventBus;

    private void Awake()
    {
        EventBus.RegisterHandler<AsteroidDestroyedEvent>(OnAsteroidDestroyed);
    }

    private void OnDestroy()
    {
        EventBus.UnregisterHandler<AsteroidDestroyedEvent>(OnAsteroidDestroyed);
    }

    private void OnAsteroidDestroyed(AsteroidDestroyedEvent obj)
    {
        Score.AddScore(obj.Score);
    }

    public void SpawnShip()
    {
        _shipController = Instantiate(_shipPrefab);
        _shipController.Health.Died += OnShipDied;
        
        _inputHandler.Setup(_shipController);

        EventBus.Publish(new ShipSpawnedEvent() { Ship = _shipController });
    }

    private void OnShipDied()
    {
        Health.Damage(1);
        _shipController.Die();
        _shipController = null;
    }
}
