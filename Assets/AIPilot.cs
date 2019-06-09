using UnityEngine;

public class AIPilot : MonoBehaviour
{
    private const float WaypointProximityTolerance = 3;
    private const float Velocity = 100 * 0.3f; // MGLT

    private Vector3 currentWaypoint;
    private int waypointIndex = 0;
    private Vector3[] waypoints = { new Vector3(-100, 0, 100), new Vector3(-100, 0, -100), new Vector3(100, 0, -100), new Vector3(100, 0, 100)};

    void Start()
    {
        currentWaypoint = waypoints[waypointIndex];
        transform.LookAt(currentWaypoint);
    }

    void Update()
    {
        if (Vector3.Distance(transform.position, currentWaypoint) <= WaypointProximityTolerance) {
            waypointIndex = waypointIndex + 1 < waypoints.Length ? waypointIndex + 1 : 0;

            // Yes, choppy :)
            currentWaypoint = waypoints[waypointIndex];
            transform.LookAt(currentWaypoint);
        } 
        transform.Translate(0, 0, Velocity * Time.deltaTime);
    }
}
