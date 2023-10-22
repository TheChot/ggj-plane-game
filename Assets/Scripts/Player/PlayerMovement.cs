using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PlayerMovement : MonoBehaviour
{
    public float throttleIncement = 0.1f;
    public float maxThrust = 200f;
    public float responsiveness = 10f;

    public float lift = 135f;

    public Transform propeller;

    float throttle;
    float roll;
    float pitch;
    float yaw;

    float responseModifier
    {
        get
        {
            return (rb.mass / 10f) * responsiveness;
        }
    }

    Rigidbody rb;
    [SerializeField]
    TextMeshProUGUI hudTxt;
    
    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    
    public void HandleInputs()
    {
        roll = Input.GetAxis("Roll");
        pitch = Input.GetAxis("Pitch");
        yaw = Input.GetAxis("Yaw");

        if (Input.GetKey(KeyCode.Space)) throttle += throttleIncement;
        else if (Input.GetKey(KeyCode.LeftShift)) throttle -= throttleIncement;
        throttle = Mathf.Clamp(throttle, 0f, 100f);

    }
    // Update is called once per frame
    void Update()
    {
        HandleInputs();
        UpdateHUD();
        
    }
    void FixedUpdate()
    {
        rb.AddForce(transform.forward * maxThrust * throttle);
        rb.AddTorque(transform.up * yaw * responseModifier);
        rb.AddTorque(transform.right * pitch * responseModifier);
        rb.AddTorque(-transform.forward * roll * responseModifier);
        rb.AddForce(Vector3.up * rb.velocity.magnitude * lift);
        propeller.Rotate(Vector3.up * throttle);
    }

    void UpdateHUD()
    {
        hudTxt.text = "Throttle: " + throttle.ToString("F0") + "%\n";
        hudTxt.text += "Airspeed: " + (rb.velocity.magnitude * 3.6f).ToString("F0") + " km/h\n";
        hudTxt.text += "Altitude: " + transform.position.y.ToString("F0") + " m";
    }
}
