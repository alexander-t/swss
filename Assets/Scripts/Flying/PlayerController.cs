using UnityEngine;

namespace Flying
{
    public class PlayerController : MonoBehaviour
    {
        public Ship ship;

        void Start()
        {
            BroadcastMessage("OnVelocityChange", new float[] { ship.Speed, ship.MaxSpeed });
        }

        void Update()
        {
            ship.BeginManeuver();
            ship.Pitch(Input.GetAxis("Vertical"));
            ship.Turn(Input.GetAxis("Horizontal"));

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
                BroadcastMessage("OnVelocityChange", new float[] { ship.Speed, ship.MaxSpeed });
            }
            else if (Input.GetKey(KeyCode.Minus) || Input.GetKey(KeyCode.KeypadMinus))
            {
                ship.DeccelerateByDelta(Time.deltaTime);
                BroadcastMessage("OnVelocityChange", new float[] { ship.Speed, ship.MaxSpeed });
            }
            else if (Input.GetKey(KeyCode.Backspace))
            {
                ship.Halt();
                BroadcastMessage("OnVelocityChange", new float[] { ship.Speed, ship.MaxSpeed });
            }

            transform.Rotate(ship.Direction * Time.deltaTime);
            transform.Translate(0, 0, ship.Speed * Time.deltaTime);
        }
    }
}