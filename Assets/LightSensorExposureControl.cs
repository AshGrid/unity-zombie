using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.Rendering.PostProcessing;

public class LightSensorExposureControl : MonoBehaviour
{
    public TextMeshProUGUI LightText;
    public PostProcessVolume postProcessVolume;
    private AutoExposure autoExposure;

    // Range for scaling exposure adjustment
    public float minExposure = -2f; // Adjust for darkest environment
    public float maxExposure = 2f;  // Adjust for brightest environment
    public float maxLightLevel = 1000f; // Approximate upper range for well-lit indoor light

    void Start()
    {
        // Enable light sensor if available
        if (LightSensor.current != null)
        {
            InputSystem.EnableDevice(LightSensor.current);
            Debug.Log("LightSensor enabled.");
        }
        else
        {
            Debug.LogWarning("LightSensor is not supported on this device.");
            LightText.text = "Sensor not supported";
        }

        // Ensure Auto Exposure is set in Post Process Volume
        if (postProcessVolume != null && postProcessVolume.profile.TryGetSettings(out autoExposure))
        {
            Debug.Log("Auto Exposure found.");
        }
        else
        {
            Debug.LogError("Auto Exposure is not set in the Post Process Volume.");
        }
    }

    void Update()
    {
        if (LightSensor.current != null && LightSensor.current.enabled)
        {
            // Read light level and display it
            float lightLevel = LightSensor.current.lightLevel.ReadValue();
            LightText.text = $"{lightLevel:F2} lux";

            // Map light level inversely to exposure:
            // Dim environment (low lux) -> higher exposure; Bright environment (high lux) -> lower exposure
            float normalizedLightLevel = Mathf.Clamp(lightLevel / maxLightLevel, 0f, 1f);
            float exposure = Mathf.Lerp(maxExposure, minExposure, normalizedLightLevel);

            // Apply calculated exposure
            if (autoExposure != null)
            {
                autoExposure.keyValue.value = exposure;
            }
        }
        else
        {
            LightText.text = "Sensor inactive";
        }
    }
}
