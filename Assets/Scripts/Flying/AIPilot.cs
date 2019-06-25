using UnityEngine;

namespace Flying
{
    [RequireComponent(typeof(Ship))]
    public class AIPilot : MonoBehaviour
    {
        private const float WaypointProximityTolerance = 10;

        public Transform[] waypoints;

        private Ship ship;

        private Vector3 currentWaypoint;
        private int waypointIndex = 0;

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
        }

        void Update()
        {
            DetermineCurrentWayPoint();
            TurnTowards(currentWaypoint);
            MoveTowards(currentWaypoint);
        }

        private void DetermineCurrentWayPoint() {
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
            transform.rotation = Quaternion.Slerp(transform.rotation, finalRotation, Time.deltaTime * 0.5f);
        }

        private void MoveTowards(Vector3 target)
        {
            transform.position += transform.forward * Time.deltaTime * ship.Velocity;
        }
    }
}