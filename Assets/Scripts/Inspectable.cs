using Core;
using System.Collections;
using UnityEngine;

/**
 * Raises an Inspected event if the player is close enough.
 **/
public class Inspectable : MonoBehaviour
{
    public float inspectionDistance = 20;
    public string contents = "Fuselage";

    private GameObject player;
    private bool inspected;

    public string VisibleContents {
        get => inspected == true ? contents : "";
    } 

    void Awake()
    {
        player = GameObject.Find(Constants.Player);
    }

    void Start()
    {
        StartCoroutine(Inspect());
    }

    IEnumerator Inspect()
    {
        while (!inspected)
        {
            if (Vector3.Distance(player.transform.position, transform.position) <= inspectionDistance)
            {
                inspected = true;
                EventManager.RaiseShipInspected(name);
            }
            yield return new WaitForSeconds(0.2f);
        }
    }
}
