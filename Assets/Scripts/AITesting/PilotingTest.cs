using UnityEngine;
using UnityEngine.UI;
using Flying;

namespace AITesting
{

    [RequireComponent(typeof(Ship))]
    public class PilotingTest : MonoBehaviour
    {
        public GameObject target;
        public Text text;

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
            Vector3 targetDirection = target.transform.position - transform.position;
            Quaternion finalRotation = Quaternion.LookRotation(targetDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, finalRotation, Time.deltaTime * ship.AngularVelocity * 0.025f);
        }

        void Move()
        {
            transform.position += transform.forward * Time.deltaTime * 20;
        }

        void OnTriggerEnter(Collider other)
        {
            GameObject targetedWithinKillzone = other.transform.root.gameObject;
            text.text = targetedWithinKillzone.name;
            if (targetedWithinKillzone == target) {
                text.text += "***";
            }
        }

        void OnTriggerExit(Collider other)
        {
            text.text = "";
        }
    }
}