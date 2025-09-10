using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(IMovementController))]
[RequireComponent(typeof(IGunController))]
public class InputHandler : MonoBehaviour
{
    private InputSystem_Actions _inputActions;
    private IMovementController _movement;
    private IGunController _gunController;
    private Vector2 _moveDirection;
    
    private void Awake()
    {
        _movement = GetComponent<IMovementController>();
        _gunController = GetComponent<IGunController>();
        
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
        
        _inputActions.Player.Attack.performed += OnShoot;
    }

    private void OnShoot(InputAction.CallbackContext obj)
    {
        _gunController.Shoot();
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        _moveDirection = context.ReadValue<Vector2>();
    }
}
