using Flying;
using UnityEngine;

public class Setup : MonoBehaviour
{
    private Transform[] waypointsA;
    private Transform[] waypointsB;
    private GameObject target;

    private GameObject xWing1;
    private AIPilot xWing1Pilot, xWing2Pilot;
    private GameObject xWing2;

    void Awake()
    {
        waypointsA = new Transform[] {
            GameObject.Find("Waypoints/Waypoint A1").transform,
            GameObject.Find("Waypoints/Waypoint A2").transform
        };

        waypointsB = new Transform[] {
            GameObject.Find("Waypoints/Waypoint B2").transform,
            GameObject.Find("Waypoints/Waypoint B1").transform
        };
        
        xWing1 = GameObject.Find("X-wing 1");
        xWing1Pilot = xWing1.GetComponent<AIPilot>();
        xWing1Pilot.waypoints = waypointsA;

        xWing2 = GameObject.Find("X-wing 2");
        xWing2Pilot = xWing2.GetComponent<AIPilot>();
        xWing2Pilot.waypoints = waypointsB;
    }


    void Update()
    {
        if (Input.GetKey(KeyCode.Alpha1))
        {
            xWing1Pilot.Attack(xWing2);
        }
        if (Input.GetKey(KeyCode.Alpha2))
        {
            xWing2Pilot.Attack(xWing1);
        }
    }
}

/*
 *                 if (Input.GetKeyUp(KeyCode.P))
                {
                    GameObject buoy = GameObject.Find("Buoys/B-55");
                    if (buoy != null)
                    {
                        behavior = new AttackingBehavior(gameObject, rightGunTransform, leftGunTransform, buoy);
                        behavior.Commence();
                    }
                }
                else if (Input.GetKeyUp(KeyCode.O))
                {
                    behavior = new PatrollingBehavior(gameObject, waypoints);
                    behavior.Commence();
                }
                else if (Input.GetKeyUp(KeyCode.Minus)) {
                    Time.timeScale = Mathf.Clamp(Time.timeScale - 0.1f, 0, 1);
                }

 */
