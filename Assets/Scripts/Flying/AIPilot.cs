using UnityEngine;

namespace Flying
{
    [RequireComponent(typeof(Ship))]
    public class AIPilot : MonoBehaviour
    {
        private const float WaypointProximityTolerance = 25;
        private const float CasualTurnDampening = 0.01f;
        private const float AttackTurnDampening = 0.025f;

        public Transform[] waypoints;
              
        private Ship ship;

        private Vector3 currentWaypoint;
        private int waypointIndex = 0;

        private PathFinding pathFinding;

        private GameObject target;
        private bool firing = false;

        void Awake()
        {
            ship = GetComponent<Ship>();
        }

        void Start()
        {
            if (waypoints.Length > 0)
            {
                currentWaypoint = waypoints[waypointIndex].position;
                transform.LookAt(currentWaypoint);
            }

            pathFinding = new PathFinding(transform);
        }

        void Update()
        {
            DetermineCurrentWayPoint();

            Direction availableDirections = pathFinding.FirstPassRaycast();

            if (availableDirections == Direction.None)
            {
                Debug.Log("STUCK! Cheating!");
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
                    TurnTowards(currentWaypoint);
                }
                MoveTowards(currentWaypoint);
            }

            if (waypointIndex == 2)
            {
                GameObject buoy = GameObject.Find("Buoys/B-55");
                target = (buoy != null) ? buoy.gameObject : null;
            }
            else
            {
                target = null;
                firing = false;
            }

            Fire();
        }


        private void Fire()
        {
            if (firing && target != null)
            {
                BroadcastMessage("OnFire");
            }
        }

        private void DetermineCurrentWayPoint()
        {
            if (waypoints.Length > 0)
            {
                if (Vector3.Distance(transform.position, currentWaypoint) <= WaypointProximityTolerance)
                {
                    waypointIndex = waypointIndex + 1 < waypoints.Length ? waypointIndex + 1 : 0;
                    currentWaypoint = waypoints[waypointIndex].position;
                }
            }
        }

        private void TurnTowards(Vector3 target)
        {
            Vector3 targetDirection = target - transform.position;
            Quaternion finalRotation = Quaternion.LookRotation(targetDirection);
            float turnDampening = target == null ? CasualTurnDampening : AttackTurnDampening;
            transform.rotation = Quaternion.Slerp(transform.rotation, finalRotation, Time.deltaTime * ship.AngularVelocity * turnDampening);
        }

        private void MoveTowards(Vector3 target)
        {
            transform.position += transform.forward * Time.deltaTime * ship.Velocity;
        }

        void OnTriggerEnter(Collider other)
        {
            if (target != null)
            {
                GameObject targetedWithinKillzone = other.transform.parent.gameObject;
                if (targetedWithinKillzone == target)
                {
                    firing = true;
                }
            }
        }

        void OnTriggerExit(Collider other)
        {
            firing = false;
        }

    }
}