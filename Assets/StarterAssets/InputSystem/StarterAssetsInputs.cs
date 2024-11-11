using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
    public class StarterAssetsInputs : MonoBehaviour
    {
        [Header("Character Input Values")]
        public Vector2 move;
        public Vector2 look;
        public bool jump;
        public bool sprint;

        [Header("Movement Settings")]
        public bool analogMovement;

        [Header("Mouse Cursor Settings")]
        public bool cursorLocked = true;
        public bool cursorInputForLook = true;

        // New variables for accelerometer-based sprinting
        private Vector3 startingAcceleration;
        public float tiltThreshold = 0.5f; // Adjust based on your needs

        private void Start()
        {
            // Store the initial accelerometer data as the baseline
            startingAcceleration = Input.acceleration;
        }

        private void Update()
        {
            // Check the accelerometer for tilt
            Vector3 currentAcceleration = Input.acceleration;
            float tiltDifference = Mathf.Sqrt(
                Mathf.Pow(currentAcceleration.x - startingAcceleration.x, 2) +
                Mathf.Pow(currentAcceleration.z - startingAcceleration.z, 2)
            );

            // Trigger or stop sprinting based on tilt
            if (tiltDifference > tiltThreshold)
            {
                SprintInput(true); // Start sprinting
            }
            else
            {
                SprintInput(false); // Stop sprinting
            }
        }

#if ENABLE_INPUT_SYSTEM
        public void OnMove(InputValue value)
        {
            MoveInput(value.Get<Vector2>());
        }

        public void OnLook(InputValue value)
        {
            if (cursorInputForLook)
            {
                LookInput(value.Get<Vector2>());
            }
        }

        public void OnJump(InputValue value)
        {
            JumpInput(value.isPressed);
        }

        public void OnSprint(InputValue value)
        {
            SprintInput(value.isPressed);
        }
#endif

        public void MoveInput(Vector2 newMoveDirection)
        {
            move = newMoveDirection;
        }

        public void LookInput(Vector2 newLookDirection)
        {
            look = newLookDirection;
        }

        public void JumpInput(bool newJumpState)
        {
            jump = newJumpState;
        }

        public void SprintInput(bool newSprintState)
        {
            sprint = newSprintState;
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            SetCursorState(cursorLocked);
        }

        private void SetCursorState(bool newState)
        {
            Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
        }
    }
}
