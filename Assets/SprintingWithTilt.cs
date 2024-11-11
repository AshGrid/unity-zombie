using UnityEngine;
using UnityEngine.InputSystem;

public class SprintingWithTilt : MonoBehaviour
{
    public float tiltThreshold = 0.5f; // The minimum tilt change from the starting position to trigger sprinting
    private Vector3 startingAcceleration;
    private bool isSprinting = false;
    private Vector3 accelerometerData;
    private PlayerInput playerInput;
    private InputAction sprintAction;

    private void Start()
    {
        // Capture the initial accelerometer data (non-sprint state)
        startingAcceleration = Input.acceleration;
        playerInput = GetComponent<PlayerInput>();

        // Get the Sprint action from the Input Actions
        sprintAction = playerInput.actions["Sprint"];
    }

    private void Update()
    {
        // Read the current accelerometer data
        accelerometerData = Input.acceleration;

        // Calculate the difference in tilt between the current and starting accelerometer data
        float tiltDifference = Mathf.Sqrt(
            Mathf.Pow(accelerometerData.x - startingAcceleration.x, 2) +
            Mathf.Pow(accelerometerData.z - startingAcceleration.z, 2)
        );

        // Check if the tilt difference exceeds the threshold
        if (tiltDifference > tiltThreshold)
        {
            TriggerSprinting(true); // Trigger sprinting if tilt is enough
        }
        else
        {
            TriggerSprinting(false); // Stop sprinting if tilt difference is not enough
        }
    }

    private void TriggerSprinting(bool shouldSprint)
    {
        if (shouldSprint != isSprinting)
        {
            isSprinting = shouldSprint;

            if (isSprinting)
            {
                Debug.Log("Sprinting started!");
                // Trigger the sprint action (simulate press)
                //sprintAction.LInvoke(); // Simulate press (perform action)
                // You can also apply speed modifications or play animations here
            }
            else
            {
                Debug.Log("Sprinting stopped!");
                // Stop the sprint action (simulate release)
                //sprintAction.Cancel(); // Simulate release (cancel action)
                // Reset speed or other attributes when sprinting stops
            }
        }
    }
}
