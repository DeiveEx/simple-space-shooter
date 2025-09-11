using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private HealthComponent _lives;
    [SerializeField] private ScoreComponent _score;
    
    private InputSystem_Actions _inputActions;

    public HealthComponent Lives => _lives;
    public ScoreComponent Score => _score;
    private IEventBus EventBus => SimpleServiceLocator.GetService<IEventBus>();
    private GameSettings GameSettings => SimpleServiceLocator.GetService<GameSettings>();

    private void Awake()
    {
        _inputActions = new InputSystem_Actions();
        _inputActions.Enable();
        
        EventBus.RegisterHandler<AsteroidDestroyedEvent>(OnAsteroidDestroyed);
    }

    private void OnDestroy()
    {
        _inputActions.Disable();
        EventBus?.UnregisterHandler<AsteroidDestroyedEvent>(OnAsteroidDestroyed);
    }
    
    public void Setup()
    {
        _lives.Setup(GameSettings.PlayerHealth);
    }

    private void OnAsteroidDestroyed(AsteroidDestroyedEvent obj)
    {
        Score.AddScore(obj.Score);
    }

    public void PossessShip(ShipController ship)
    {
        ship.Health.Died += OnShipDied;
        ship.GetComponent<IInputHandler>().Setup(_inputActions);
    }

    private void OnShipDied()
    {
        //When the ship dies, the player loses a life
        Lives.Damage(1);
    }
}
