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
    private IEventBus EventBus => SimpleServiceLocator.GetService<IEventBus>();
    private GameSettings GameSettings => SimpleServiceLocator.GetService<GameSettings>();

    private void Awake()
    {
        _shipController = Instantiate(_shipPrefab);
        _shipController.gameObject.SetActive(false);
        
        EventBus.RegisterHandler<AsteroidDestroyedEvent>(OnAsteroidDestroyed);
    }

    private void OnDestroy()
    {
        EventBus?.UnregisterHandler<AsteroidDestroyedEvent>(OnAsteroidDestroyed);
    }
    
    public void Setup()
    {
        _health.Setup(GameSettings.PlayerHealth);
    }

    private void OnAsteroidDestroyed(AsteroidDestroyedEvent obj)
    {
        Score.AddScore(obj.Score);
    }

    public void SpawnShip()
    {
        _shipController.gameObject.SetActive(true);
        _shipController.Setup();
        _shipController.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
        _shipController.Health.Died += OnShipDied;
        
        _inputHandler.Setup(_shipController);

        EventBus.Publish(new ShipSpawnedEvent() { Ship = _shipController });
    }

    private void OnShipDied()
    {
        Health.Damage(1);
        _shipController.Die();
    }
}
