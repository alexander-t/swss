using Core;
using System.Collections;
using UnityEngine;

/**
 * Raises an Inspected event if the player is close enough.
 **/
public class Inspectable : MonoBehaviour
{
    public float inspectionDistance = 20;

    private GameObject player;
    private bool inspected;

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
                EventManager.RaiseShipInspected(name);
                inspected = true;
            }
            yield return new WaitForSeconds(0.2f);
        }
    }
}
