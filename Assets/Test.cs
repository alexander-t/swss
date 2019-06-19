using UnityEngine;
using UnityEngine.UI;
using Flying;

public class Test : MonoBehaviour
{
    public Text text;
    public Text text2;

    private Vector3 target = new Vector3(0, 20, 100);
    private Ship ship;
    private Vector3 turnStartDirection;

    private void Start()
    {
        ship = GetComponent<Ship>();
    }

    void Update()
    {
        Vector3 diff = target - transform.position;
        float angle = Vector3.Angle(transform.forward, target);
        Debug.DrawLine(transform.position, transform.forward * 100, Color.green, 0.1f);
        Debug.DrawLine(transform.position, target, Color.red, 0.1f);

        text.text = diff.y + "";
        text2.text = angle + "";



        ship.BeginManeuver();
        if (diff.y > 0.5)
        {
            if (angle > 5)
            {
                ship.PitchUp();
            }
        }
        else {
            transform.rotation = Quaternion.identity;
        }
    }
}

