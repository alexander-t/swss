using Flying;
using UnityEngine;

namespace AI
{
    public class PatrollingBehavior : Behavior
    {
        private const float WaypointProximityTolerance = 25;
        private const float TurnDampening = 0.01f;

        private Transform managedTransform;
        private Ship ship;

        private Transform[] waypoints;
        private int waypointIndex;
        private Vector3 currentWaypoint = Vector3.zero;

        public PatrollingBehavior(GameObject managedShip, Transform[] waypoints)
        {
            this.waypoints = waypoints;
            managedTransform = managedShip.transform;
            ship = managedShip.GetComponent<Ship>();
        }

        public void Commence()
        {
            waypointIndex = 0;
            if (waypoints?.Length > 0)
            {
                currentWaypoint = waypoints[waypointIndex].position;
            }
            else
            {
                ship.Halt();
            }
        }

        public void Turn()
        {
            DetermineCurrentWayPoint();
            TurnTowards(currentWaypoint);
        }

        public void Attack()
        {
            // Do nothing
        }

        public string Describe()
        {
            if (waypoints == null || waypoints.Length == 0)
            {
                return ship.Name + ": Patrolling -> no waypoints specified";
            }
            return ship.Name + ": Patrolling -> going to waypoint #" + waypointIndex + " " + waypoints[waypointIndex].position;
        }

        public void TargetEnteredKillzone(GameObject potentialTarget)
        {
            // Do nothing
        }

        public void TargetLeftKillzone(GameObject potentialTarget)
        {
            // Do nothing
        }

        private void DetermineCurrentWayPoint()
        {
            if (waypoints?.Length > 0)
            {
                if (Vector3.Distance(managedTransform.position, currentWaypoint) <= WaypointProximityTolerance)
                {
                    waypointIndex = waypointIndex + 1 < waypoints.Length ? waypointIndex + 1 : 0;
                    currentWaypoint = waypoints[waypointIndex].position;
                }
            }
        }

        private void TurnTowards(Vector3 target)
        {
            Vector3 targetDirection = target - managedTransform.position;
            Quaternion finalRotation = Quaternion.LookRotation(targetDirection);
            managedTransform.rotation = Quaternion.Slerp(managedTransform.rotation, finalRotation, Time.deltaTime * ship.AngularVelocity * TurnDampening);
        }
    }
}

