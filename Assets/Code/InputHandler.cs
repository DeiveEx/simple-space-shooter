using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(IMovementController))]
public class InputHandler : MonoBehaviour
{
    private InputSystem_Actions _inputActions;
    private IMovementController _movement;
    private Vector2 _moveDirection;
    
    private void Awake()
    {
        _movement = GetComponent<IMovementController>();
        
        _inputActions = new InputSystem_Actions();
        RegisterInputEvents();
        _inputActions.Enable();
    }

    private void Update()
    {
        _movement.SetDirection(_moveDirection);
    }

    private void RegisterInputEvents()
    {
        _inputActions.Player.Move.performed += OnMove;
        _inputActions.Player.Move.canceled += OnMove;
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        _moveDirection = context.ReadValue<Vector2>();
    }
}
