using UnityEngine;

public class ForwardMovement : MonoBehaviour, IMovementController
{
    [SerializeField] private float _speed = 5;
    [SerializeField] private Vector2 _initialDirection = Vector2.up;

    private Vector2 _moveDirection;

    private void Awake()
    {
        _moveDirection = _initialDirection;
    }

    public void SetDirection(Vector2 direction)
    {
        _moveDirection = direction.normalized;
    }

    private void Update()
    {
        transform.Translate(_moveDirection * (_speed * Time.deltaTime));
    }
}
