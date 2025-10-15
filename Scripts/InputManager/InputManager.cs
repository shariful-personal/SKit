using UnityEngine;
using UnityEngine.InputSystem;

namespace SKit
{
    public static class InputData
    {
        public static Vector2 MoveInput;
        public static bool IsSprinting;
        public static bool IsJumping;
    }

    public class InputManager : MonoBehaviour
    {
        [SerializeField] private InputActionReference moveAction;
        [SerializeField] private InputActionReference sprintAction;
        [SerializeField] private InputActionReference jumpAction;

        private void OnEnable()
        {
            SubscribeToMove(true);
            SubscribeToSprint(true);
            SubscribeToJump(true);
        }

        private void OnDisable()
        {
            SubscribeToMove(false);
            SubscribeToSprint(false);
            SubscribeToJump(false);
        }

        private void OnMove(InputAction.CallbackContext context)
        {
            InputData.MoveInput = context.ReadValue<Vector2>();
        }

        private void OnSprint(InputAction.CallbackContext context)
        {
            InputData.IsSprinting = context.ReadValueAsButton();
        }

        private void OnJump(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                InputData.IsJumping = true;
            }
        }

        private void SubscribeToMove(bool isEnable)
        {
            if (isEnable)
            {
                moveAction.action.performed += OnMove;
                moveAction.action.canceled += OnMove;
                moveAction.action.Enable();
            }
            else
            {
                moveAction.action.performed -= OnMove;
                moveAction.action.canceled -= OnMove;
                moveAction.action.Disable();
            }
        }

        private void SubscribeToSprint(bool isEnable)
        {
            if (isEnable)
            {
                sprintAction.action.performed += OnSprint;
                sprintAction.action.canceled += OnSprint;
                sprintAction.action.Enable();
            }
            else
            {
                sprintAction.action.performed -= OnSprint;
                sprintAction.action.canceled -= OnSprint;
                sprintAction.action.Disable();
            }
        }

        private void SubscribeToJump(bool isEnable)
        {
            if (isEnable)
            {
                jumpAction.action.performed += OnJump;
                jumpAction.action.Enable();
            }
            else
            {
                jumpAction.action.performed -= OnJump;
                jumpAction.action.Disable();
            }
        }
    }
}