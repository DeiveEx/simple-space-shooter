using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(ShipController))]
public class ShipInputHandler : MonoBehaviour, IInputHandler
{
    private InputSystem_Actions _inputActions;
    private Vector2 _moveDirection;
    
    private InputSystem_Actions _inputSystemActions;
    private ShipController _shipController;

    private void Awake()
    {
        _shipController = GetComponent<ShipController>();
    }

    private void Update()
    {
        if(_shipController == null)
            return;
        
        _shipController.Movement.SetDirection(_moveDirection);
    }

    public void Setup(InputSystem_Actions inputActions)
    {
        _inputActions = inputActions;
        RegisterInputEvents();
    }

    private void OnDestroy()
    {
        if(_inputActions != null)
            UnregisterInputEvents();
    }

    private void RegisterInputEvents()
    {
        _inputActions.Player.Move.performed += OnMove;
        _inputActions.Player.Move.canceled += OnMove;
    }
    
    private void UnregisterInputEvents()
    {
        _inputActions.Player.Move.performed -= OnMove;
        _inputActions.Player.Move.canceled -= OnMove;
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        _moveDirection = context.ReadValue<Vector2>();
    }
}
