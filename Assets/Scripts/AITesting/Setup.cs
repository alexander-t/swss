using Flying;
using UnityEngine;

public class Setup : MonoBehaviour
{
    private Transform waypoint1;
    private Transform waypoint2;
    private GameObject target;

    void Awake()
    {
        waypoint1 = GameObject.Find("Waypoints/Waypoint 1").transform;
        waypoint2 = GameObject.Find("Waypoints/Waypoint 2").transform;
        target = GameObject.Find("Buoys/B-55");
    }

    void Start()
    {
        GetComponent<Ship>().Halt();    
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Alpha1))
        {
            GetComponent<Ship>().Halt();
            transform.position = waypoint1.position;
            transform.LookAt(target.transform);
        }
        else if (Input.GetKeyUp(KeyCode.Q))
        {
            GetComponent<Ship>().Halt();
            transform.position = waypoint2.position;
            transform.Rotate(0, 90, 0);
        }

        else if (Input.GetKey(KeyCode.Space))
        {
            Time.timeScale = 0;
        }
        Debug.DrawLine(transform.position, transform.position + transform.forward * 100, Color.white);
    }
}
