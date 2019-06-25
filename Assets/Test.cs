using UnityEngine;
using UnityEngine.UI;
using Flying;

[RequireComponent(typeof(Ship))]
public class Test : MonoBehaviour
{
    public Transform target;
    public Text text;
    public Text text2;

    private Ship ship;
    private Vector3 turnStartDirection;

    void Awake()
    {
        ship = GetComponent<Ship>();
    }

    void Update()
    {
        Turn();
        Move();
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

