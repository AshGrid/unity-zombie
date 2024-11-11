using UnityEngine;
using UnityEngine.InputSystem;

public class Compass : MonoBehaviour
{
    public RectTransform compassNeedle; // The UI element representing the compass needle

    void Start()
    {
        // Check if the AttitudeSensor is supported and enable it
        if (AttitudeSensor.current != null)
        {
            InputSystem.EnableDevice(AttitudeSensor.current);
            Debug.Log("AttitudeSensor enabled successfully.");
        }
        else
        {
            Debug.LogWarning("AttitudeSensor is not supported on this device.");
        }
    }

    void Update()
    {
        // Ensure the sensor is available and enabled
        if (AttitudeSensor.current != null && AttitudeSensor.current.enabled)
        {
            // Read the current attitude value
            Quaternion deviceAttitude = AttitudeSensor.current.attitude.ReadValue();
            Debug.Log("Device Attitude Read: " + deviceAttitude);

            // Correct the quaternion to account for the phone's orientation
            // Adjust to treat the device as flat (horizontally facing upward)
            Quaternion correction = Quaternion.Euler(90, 0, 0);
            Quaternion correctedAttitude = correction * deviceAttitude;

            // Extract the heading from the adjusted rotation
            Vector3 euler = correctedAttitude.eulerAngles;
            float compassHeading = euler.y;

            // Rotate the compass needle; negative to rotate clockwise in the UI
            compassNeedle.rotation = Quaternion.Euler(0, 0, -compassHeading);
            Debug.Log("Compass Heading: " + compassHeading);
        }
        else
        {
            Debug.LogWarning("AttitudeSensor is either not detected or not enabled.");
        }
    }
}
