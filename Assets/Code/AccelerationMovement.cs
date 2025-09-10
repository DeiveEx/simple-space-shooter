using System;
using UnityEngine;

public class AccelerationMovement : MonoBehaviour, IMovementController
{
    [Serializable]
    public struct MovementSettings
    {
        public float Acceleration;
        public float MaxSpeed;
        public float Drag;
    }
    
    [SerializeField] private bool _autoSetup;
    [SerializeField] private MovementSettings _settings;
    
    private Rigidbody2D _rigidbody;
    private Vector2 _forceDirection;
    
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        
        if(_autoSetup)
            Setup(_settings);
    }

    private void FixedUpdate()
    {
        ApplyForce(_forceDirection * _settings.Acceleration);
        _forceDirection = Vector2.zero;
    }

    public void Setup(MovementSettings settings)
    {
        _settings = settings;
        
        _rigidbody.linearDamping = _settings.Drag;
    }

    public void Move(Vector2 direction)
    {
        _forceDirection = direction.normalized;
    }

    private void ApplyForce(Vector2 force)
    {
        _rigidbody.AddForce(force);
        _rigidbody.linearVelocity = Vector2.ClampMagnitude(_rigidbody.linearVelocity, _settings.MaxSpeed);
    }
}
