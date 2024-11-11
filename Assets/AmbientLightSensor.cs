using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientLightSensor : MonoBehaviour
{
    private AndroidJavaObject _sensorManager;
    private AndroidJavaObject _lightSensor;
    private AndroidJavaObject _sensorEventListener;

    private void Start()
    {
        Debug.Log("Initializing Ambient Light Sensor script...");

        if (Application.platform == RuntimePlatform.Android)
        {
            try
            {
                Debug.Log("Running on Android platform, setting up sensor...");

                // Get the Android Activity
                using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
                {
                    AndroidJavaObject activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
                    _sensorManager = activity.Call<AndroidJavaObject>("getSystemService", "sensor");

                    if (_sensorManager == null)
                    {
                        Debug.LogError("Failed to get SensorManager.");
                        return;
                    }

                    // Get the ambient light sensor (Sensor type 5)
                    _lightSensor = _sensorManager.Call<AndroidJavaObject>("getDefaultSensor", 5); // 5 = ambient light sensor type

                    if (_lightSensor != null)
                    {
                        Debug.Log("Ambient light sensor found. Registering listener...");
                        _sensorEventListener = new AndroidJavaObject("android.hardware.SensorEventListener", new SensorEventListenerCallback(OnLightSensorChanged));
                        _sensorManager.Call("registerListener", _sensorEventListener, _lightSensor, 3); // 3 = SENSOR_DELAY_NORMAL
                        Debug.Log("Ambient light sensor listener registered successfully.");
                    }
                    else
                    {
                        Debug.LogWarning("No ambient light sensor found on this device.");
                    }
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError("Error initializing ambient light sensor: " + ex.Message);
            }
        }
        else
        {
            Debug.LogWarning("Ambient light sensor is only supported on Android devices.");
        }
    }

    private void OnLightSensorChanged(float lightLevel)
    {
        Debug.Log("Ambient light level changed. Light level: " + lightLevel);
        // Add your game logic here based on light level, such as adjusting brightness or gameplay mechanics.
    }

    private void OnDestroy()
    {
        Debug.Log("Destroying Ambient Light Sensor script, unregistering listener...");

        if (Application.platform == RuntimePlatform.Android && _sensorManager != null && _lightSensor != null)
        {
            _sensorManager.Call("unregisterListener", _sensorEventListener, _lightSensor);
            Debug.Log("Ambient light sensor listener unregistered.");
        }
    }

    // Callback class to handle sensor events
    class SensorEventListenerCallback : AndroidJavaProxy
    {
        private readonly System.Action<float> _onSensorChanged;

        public SensorEventListenerCallback(System.Action<float> onSensorChanged)
            : base("android.hardware.SensorEventListener")
        {
            Debug.Log("SensorEventListenerCallback initialized.");
            _onSensorChanged = onSensorChanged;
        }

        public void onSensorChanged(AndroidJavaObject sensorEvent)
        {
            Debug.Log("onSensorChanged called in Java with event data.");
            float lightLevel = sensorEvent.Get<float[]>("values")[0];
            Debug.Log("Light level extracted: " + lightLevel);
            _onSensorChanged?.Invoke(lightLevel);
        }

        public void onAccuracyChanged(AndroidJavaObject sensor, int accuracy)
        {
            Debug.Log("Sensor accuracy changed: " + accuracy);
        }
    }
}
