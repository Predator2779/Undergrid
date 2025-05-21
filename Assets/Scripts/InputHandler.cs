using UnityEngine;
using UnityEngine.InputSystem;

namespace DefaultNamespace
{
    public class InputHandler : MonoBehaviour
    {
        [SerializeField] private PlayerMovement _movement;
        
        private PlayerInputActions _inputActions;
        
        private void Awake()
        {
            _inputActions = new PlayerInputActions();
        }
        private void OnEnable()
        {
            _inputActions.Enable();
            _inputActions.Player.Move.performed += SetDirection;
            _inputActions.Player.Move.canceled += SetDirection;
        }

        private void OnDisable()
        {
            _inputActions.Disable();
            _inputActions.Player.Move.performed -= SetDirection;
            _inputActions.Player.Move.canceled -= SetDirection;
        }

        private void SetDirection(InputAction.CallbackContext context)
        {
            _movement.InputDirection = context.ReadValue<Vector2>();
        }
    }
}