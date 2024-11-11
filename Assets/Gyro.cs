using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Gyro : MonoBehaviour
{
    Vector3 rot;
    // Start is called before the first frame update
    void Start()
    {
        rot = Vector3.zero;
        Input.gyro.enabled = true;
        
    }

    // Update is called once per frame
    void Update()
    {
        rot.y = Input.gyro.rotationRateUnbiased.y;
        transform.Rotate(- rot);
        Debug.Log(Input.gyro.attitude);
    }
}
