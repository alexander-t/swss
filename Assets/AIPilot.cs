using UnityEngine;

namespace Flying
{
    public class AIPilot : MonoBehaviour
    {
        private const float WaypointProximityTolerance = 3;

        public float velocity;
        public Vector3[] waypoints;

        private Ship ship;

        private Vector3 currentWaypoint;
        private int waypointIndex = 0;

        void Start()
        {
            ship = GetComponent<Ship>();

            if (waypoints.Length > 0)
            {
                currentWaypoint = waypoints[waypointIndex];
                transform.LookAt(currentWaypoint);
            }
        }

        void Update()
        {
            if (waypoints.Length > 0)
            {
                if (Vector3.Distance(transform.position, currentWaypoint) <= WaypointProximityTolerance)
                {
                    waypointIndex = waypointIndex + 1 < waypoints.Length ? waypointIndex + 1 : 0;

                    // Yes, choppy :)
                    currentWaypoint = waypoints[waypointIndex];
                    transform.LookAt(currentWaypoint);
                }
            }

            transform.Rotate(ship.Direction * Time.deltaTime);
            transform.Translate(0, 0, velocity * Time.deltaTime);
        }
    }
}