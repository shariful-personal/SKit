// PlayerMovement.cs
using UnityEngine;

namespace SKit
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] float moveSpeed = 5f;
        [SerializeField] float runSpeed = 10f;
        [SerializeField] float jumpHeight = 1.8f;
        [SerializeField] float gravity = -9.81f;
        [SerializeField] float groundedGravity = -2f;
        [SerializeField] float rotationSmoothTime = 0.1f;
        [SerializeField] private Transform cameraTransform;

        private CharacterController controller;
        private Vector3 verticalVelocity, moveDirection;

        private void Awake()
        {
            controller = GetComponent<CharacterController>();
        }

        private void Update()
        {
            HandleMovement();
            HandleJumpAndGravity();
        }

        private void HandleMovement()
        {
            float inputMagnitude = Mathf.Clamp01(InputData.MoveInput.magnitude);
            

            if (inputMagnitude > 0.01f)
            {
                RotatePlayer();
                MovePlayer(inputMagnitude);
            }
        }

        private void RotatePlayer()
        {
            if (InputData.MoveInput.x == 0f)
            {
                transform.rotation = Quaternion.Slerp(
                    transform.rotation, 
                    Quaternion.Euler(0f, cameraTransform.rotation.eulerAngles.y, 0f), 
                    Mathf.Clamp01(Time.deltaTime / rotationSmoothTime)
                );
            }
            else
            {
                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    Quaternion.Euler(0f, Mathf.Atan2(InputData.MoveInput.x, InputData.MoveInput.y) * Mathf.Rad2Deg + cameraTransform.rotation.eulerAngles.y, 0f),
                    Mathf.Clamp01(Time.deltaTime / rotationSmoothTime)
                );
            }
        }

        private void MovePlayer(float inputMagnitude)
        {
            moveDirection = cameraTransform.forward * InputData.MoveInput.y + cameraTransform.right * InputData.MoveInput.x;
            float speed = InputData.IsSprinting ? runSpeed : moveSpeed;
            controller.Move(inputMagnitude * speed * Time.deltaTime * moveDirection.normalized);
        }

        private void HandleJumpAndGravity()
        {
            bool grounded = controller.isGrounded;
            if (grounded && verticalVelocity.y < 0f)
            {
                verticalVelocity.y = groundedGravity;
            }

            if (grounded && InputData.IsJumping)
            {
                InputData.IsJumping = false;
                verticalVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }

            verticalVelocity.y += gravity * Time.deltaTime;
            controller.Move(verticalVelocity * Time.deltaTime);
        }
    }
}