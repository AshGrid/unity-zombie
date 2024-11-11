using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;

public class Acc : MonoBehaviour
{
    private Rigidbody rb;
    public float speed = 5000f;
    Vector3 movement;
    // Start is called before the first frame update
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    void Start()
    {
      
         //movement = Vector3.zero;
        //Input.acceleration.Normalize();
    }

    void Main()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
         movement.z = Input.acceleration.z;
         movement.x = Input.acceleration.x;
         //Vector3 movement = new Vector3(Input.acceleration.x, Input.acceleration.y, Input.acceleration.z);
         //transform.position = movement * Time.deltaTime;
         rb.AddForce( movement * speed * Time.deltaTime);

        // rb.AddForce(movement*speed*Time.deltaTime);
    }
}
