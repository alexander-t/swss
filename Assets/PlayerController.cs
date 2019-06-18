using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Ship-specific
    private const float MaxVelocity = 100f;
    private const float Acceleration = 10f;
    private const float AngularVelocity = 50f;

    private float velocity = 20f;

    void Start()
    {
        BroadcastMessage("OnVelocityChange", new float[] { velocity, MaxVelocity });
    }

    void Update()
    {
        Vector3 direction = new Vector3();

        if (Input.GetKey(KeyCode.W))
        {
            direction.x = AngularVelocity;
        }
        if (Input.GetKey(KeyCode.S))
        {
            direction.x -= AngularVelocity;
        }
        if (Input.GetKey(KeyCode.A))
        {
            direction.y -= AngularVelocity;
            direction.z += AngularVelocity;
        }
        if (Input.GetKey(KeyCode.D))
        {
            direction.y += AngularVelocity;
            direction.z -= AngularVelocity;
        }
        if (Input.GetKey(KeyCode.E))
        {
            direction.z -= AngularVelocity;
        }
        if (Input.GetKey(KeyCode.Q))
        {
            direction.z += AngularVelocity;
        }

        // Stupid work-around for Swedish keyboard layout
        if (Input.GetKey(KeyCode.Equals) || Input.GetKey(KeyCode.KeypadPlus))
        {
            velocity += Acceleration * Time.deltaTime;
            BroadcastMessage("OnVelocityChange", new float[] { velocity, MaxVelocity });
        }
        else if (Input.GetKey(KeyCode.Minus) || Input.GetKey(KeyCode.KeypadMinus))
        {
            velocity -= Acceleration * Time.deltaTime;
            BroadcastMessage("OnVelocityChange", new float[] { velocity, MaxVelocity });
        }
        else if (Input.GetKey(KeyCode.Backspace))
        {
            velocity = 0;
            BroadcastMessage("OnVelocityChange", new float[] { velocity, MaxVelocity });
        }

        velocity = Mathf.Clamp(velocity, 0, MaxVelocity);
        direction *= Time.deltaTime;
        transform.Rotate(direction);
        transform.Translate(0, 0, velocity * Time.deltaTime);
    }
}
