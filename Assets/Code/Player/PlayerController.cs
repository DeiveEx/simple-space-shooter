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
    
    public void SpawnShip()
    {
        _shipController = Instantiate(_shipPrefab);
        _shipController.Health.Died += () => Health.Damage(1);
        
        _inputHandler.Setup(_shipController);
    }
}
