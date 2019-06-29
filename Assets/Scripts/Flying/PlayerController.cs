using UnityEngine;

namespace Flying
{
    public class PlayerController : MonoBehaviour
    {
        private Ship ship;

        void Awake()
        {
            ship = GetComponent<Ship>();
        }

        void Start()
        {
            BroadcastMessage("OnVelocityChange", new float[] { ship.Velocity, ship.MaxSpeed });
        }

        void Update()
        {
            ship.BeginManeuver();
            if (Input.GetKey(KeyCode.W))
            {
                ship.PitchDown();
            }
            if (Input.GetKey(KeyCode.S))
            {
                ship.PitchUp();
            }
            if (Input.GetKey(KeyCode.A))
            {
                ship.TurnLeft();
            }
            if (Input.GetKey(KeyCode.D))
            {
                ship.TurnRight();
            }
            if (Input.GetKey(KeyCode.E))
            {
                ship.RollRight();
            }
            if (Input.GetKey(KeyCode.Q))
            {
                ship.RollLeft();
            }

            // Stupid work-around for Swedish keyboard layout
            if (Input.GetKey(KeyCode.Equals) || Input.GetKey(KeyCode.KeypadPlus))
            {
                ship.AccelerateByDelta(Time.deltaTime);
                BroadcastMessage("OnVelocityChange", new float[] { ship.Velocity, ship.MaxSpeed });
            }
            else if (Input.GetKey(KeyCode.Minus) || Input.GetKey(KeyCode.KeypadMinus))
            {
                ship.DeccelerateByDelta(Time.deltaTime);
                BroadcastMessage("OnVelocityChange", new float[] { ship.Velocity, ship.MaxSpeed });
            }
            else if (Input.GetKey(KeyCode.Backspace))
            {
                ship.Halt();
                BroadcastMessage("OnVelocityChange", new float[] { ship.Velocity, ship.MaxSpeed });
            }

            transform.Rotate(ship.Direction * Time.deltaTime);
            transform.Translate(0, 0, ship.Velocity * Time.deltaTime);
        }
    }
}