using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    private InputSystem_Actions _inputActions;
    private Vector2 _moveDirection;
    
    private ShipController _shipController;
    
    private void Awake()
    {
        _shipController = GetComponent<ShipController>();
        
        _inputActions = new InputSystem_Actions();
        RegisterInputEvents();
        _inputActions.Enable();
    }

    private void Update()
    {
        if(_shipController == null)
            return;
        
        _shipController.Movement.SetDirection(_moveDirection);
    }

    public void Setup(ShipController shipController)
    {
        _shipController = shipController;
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
