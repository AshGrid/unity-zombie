using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class Test : MonoBehaviour
{
    public TextMeshProUGUI LightText;

    // Start is called before the first frame update
    void Start()
    {
        if (LightSensor.current != null)
        {
            InputSystem.EnableDevice(LightSensor.current);
        }
        else
        {
            Debug.LogWarning("LightSensor is not supported on this device.");
            LightText.text = "Sensor not supported";
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (LightSensor.current != null && LightSensor.current.enabled)
        {
            // Get the current light level
            float lightLevel = LightSensor.current.lightLevel.ReadValue();
            LightText.text = lightLevel.ToString("F2") + " lux";
        }
        else
        {
            LightText.text = "Sensor inactive";
        }
    }
}
