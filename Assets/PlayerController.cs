using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    // Configurable from inspector 
    public float angularVelocity;
    public Text text;

    // Ship-specific
    private const float MaxVelocity = 10f;
    private const float Acceleration = 2f;

    private float velocity = 4f;

    void Update()
    {
        Vector3 direction = new Vector3();

        if (Input.GetKey(KeyCode.W))
        {
            direction.x = angularVelocity;
        }
        if (Input.GetKey(KeyCode.S))
        {
            direction.x -= angularVelocity;
        }
        if (Input.GetKey(KeyCode.A))
        {
            direction.y -= angularVelocity;
            direction.z += angularVelocity;
        }
        if (Input.GetKey(KeyCode.D))
        {
            direction.y += angularVelocity;
            direction.z -= angularVelocity;
        }
        if (Input.GetKey(KeyCode.E))
        {
            direction.z -= angularVelocity;
        }
        if (Input.GetKey(KeyCode.Q))
        {
            direction.z += angularVelocity;
        }

        // Stupid work-around for Swedish keyboard layout
        if (Input.GetKey(KeyCode.Equals) || Input.GetKey(KeyCode.KeypadPlus))
        {
            velocity += Acceleration * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.Minus) || Input.GetKey(KeyCode.KeypadMinus))
        {
            velocity -= Acceleration * Time.deltaTime;
        }
        velocity = Mathf.Clamp(velocity, 0, MaxVelocity);
        direction *= Time.deltaTime;
        transform.Rotate(direction);
        transform.Translate(0, 0, velocity * Time.deltaTime);

        text.text = "Velocity: " + velocity;
    }
        
}
