using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShipController : MonoBehaviour
{
    [SerializeField] private float _acceleration = 1f;
    [SerializeField] private float _maxSpeed = 5f;
    [SerializeField] private float _drag = 5f;

    private Rigidbody2D _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _rigidbody.linearDamping = _drag;
    }

    private void Update()
    {
        if(Keyboard.current.aKey.isPressed)
            _rigidbody.AddForce(Vector2.left * _acceleration);
        
        if(Keyboard.current.dKey.isPressed)
            _rigidbody.AddForce(Vector2.right * _acceleration);
        
        if(Keyboard.current.wKey.isPressed)
            _rigidbody.AddForce(Vector2.up * _acceleration);
        
        if(Keyboard.current.sKey.isPressed)
            _rigidbody.AddForce(Vector2.down * _acceleration);
    }

    private void FixedUpdate()
    {
        _rigidbody.linearVelocity = Vector2.ClampMagnitude(_rigidbody.linearVelocity, _maxSpeed);
    }
}
