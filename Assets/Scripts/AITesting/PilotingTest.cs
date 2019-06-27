using UnityEngine;
using Flying;

namespace AITesting
{

    [RequireComponent(typeof(Ship))]
    public class PilotingTest : MonoBehaviour
    {
        public Transform target;

        private Ship ship;
        private PathFinding pathFinding;


        void Awake()
        {
            ship = GetComponent<Ship>();
        }

        void Start()
        {
            pathFinding = new PathFinding(transform);
        }

        void Update()
        {
            Direction availableDirections = pathFinding.FirstPassRaycast();

            if (availableDirections == Direction.None)
            {
                Debug.Log("Stuck! Cheating...");
                transform.Rotate(0, 180, 0);
            }
            else
            {
                Vector3 turningDirection = pathFinding.DetermineTurningDirection(availableDirections);
                if (turningDirection != Vector3.zero)
                {
                    transform.Rotate(turningDirection * ship.AngularVelocity * Time.deltaTime);
                }
                else
                {
                    Turn();
                }
                Move();
            }
        }

        void Turn()
        {
            Vector3 targetDirection = target.position - transform.position;
            Quaternion finalRotation = Quaternion.LookRotation(targetDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, finalRotation, Time.deltaTime * 0.5f);
        }

        void Move()
        {
            transform.position += transform.forward * Time.deltaTime * 20;
        }
    }
}