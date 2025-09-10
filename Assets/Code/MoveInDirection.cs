using UnityEngine;

[RequireComponent(typeof(IMovementController))]
public class MoveInDirection : MonoBehaviour
{
    [SerializeField] private bool _useRandomInitialDirection;
    
    private IMovementController _movement;
    private Vector2 _direction;

    private void Awake()
    {
        _movement = GetComponent<IMovementController>();
        
        if(_useRandomInitialDirection)
            _direction = Random.insideUnitCircle;
    }

    private void Update()
    {
        _movement.Move(_direction);
    }
    
    public void SetDirection(Vector2 direction)
    {
        _direction = direction;
    }
}
