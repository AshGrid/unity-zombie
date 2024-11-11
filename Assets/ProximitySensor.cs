using UnityEngine;
using TMPro; // Import TextMeshPro namespace

public class ProximitySensor : MonoBehaviour
{
    private AndroidJavaObject _sensorManager;
    private AndroidJavaObject _proximitySensor;
    private AndroidJavaProxy _sensorEventListener;

    // Reference to the UI TextMeshProUGUI component
    public TextMeshProUGUI proximityText;

    private void Start()
    {
        Debug.Log("Initializing Proximity Sensor script...");

        if (proximityText == null)
        {
            Debug.LogError("Proximity Text UI element is not assigned.");
            return;
        }

        if (Application.platform == RuntimePlatform.Android)
        {
            try
            {
                Debug.Log("Running on Android platform, setting up sensor...");
                using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
                {
                    AndroidJavaObject activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
                    _sensorManager = activity.Call<AndroidJavaObject>("getSystemService", "sensor");

                    if (_sensorManager == null)
                    {
                        Debug.LogError("Failed to get SensorManager.");
                        return;
                    }

                    _proximitySensor = _sensorManager.Call<AndroidJavaObject>("getDefaultSensor", 8); // 8 = TYPE_PROXIMITY

                    if (_proximitySensor != null)
                    {
                        Debug.Log("Proximity sensor found. Registering listener...");
                        _sensorEventListener = new SensorEventListener(OnProximityChanged);
                        _sensorManager.Call("registerListener", _sensorEventListener, _proximitySensor, 3); // SENSOR_DELAY_NORMAL
                        Debug.Log("Proximity sensor listener registered successfully.");
                    }
                    else
                    {
                        Debug.LogWarning("No proximity sensor found on this device.");
                    }
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError("Error initializing proximity sensor: " + ex.Message);
            }
        }
        else
        {
            Debug.LogWarning("Proximity sensor is only supported on Android devices.");
        }
    }

    private void OnProximityChanged(float distance)
    {
        Debug.Log("Proximity sensor triggered. Distance: " + distance);
        // Display the distance on the UI
        if (proximityText != null)
        {
            proximityText.text = "Proximity Distance: " + distance.ToString("F2") + " cm";
        }
    }

    private void OnDestroy()
    {
        Debug.Log("Destroying Proximity Sensor script, unregistering listener...");

        if (Application.platform == RuntimePlatform.Android && _sensorManager != null && _proximitySensor != null)
        {
            _sensorManager.Call("unregisterListener", _sensorEventListener, _proximitySensor);
            Debug.Log("Proximity sensor listener unregistered.");
        }
    }

    // Callback class to handle sensor events
    class SensorEventListener : AndroidJavaProxy
    {
        private readonly System.Action<float> _onSensorChanged;

        public SensorEventListener(System.Action<float> onSensorChanged)
            : base("android.hardware.SensorEventListener")
        {
            Debug.Log("SensorEventListener initialized.");
            _onSensorChanged = onSensorChanged;
        }

        public void onSensorChanged(AndroidJavaObject sensorEvent)
        {
            float distance = sensorEvent.Get<float[]>("values")[0];
            _onSensorChanged?.Invoke(distance);
        }

        public void onAccuracyChanged(AndroidJavaObject sensor, int accuracy)
        {
            Debug.Log("Sensor accuracy changed: " + accuracy);
        }
    }
}
