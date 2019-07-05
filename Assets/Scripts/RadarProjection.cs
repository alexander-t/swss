using Core;
using UnityEngine;

/**
 * Projects an object towards the player at a constant distance.
 * The projected object is to be picked up a radar camera.
 **/
public class RadarProjection : MonoBehaviour
{
    [Tooltip("Object that will be picked up by the radar camera. ")]
    public GameObject radarSignatureObject;

    private const float DistanceToRadar = 10;

    private GameObject player;
    private Vector3 direction;

    void Start()
    {
        player = GameObject.Find(Constants.Player);
    }

    void Update()
    {
        direction = transform.position - player.transform.position ;
        direction.Normalize();
        radarSignatureObject.transform.position = player.transform.position + direction * DistanceToRadar;
    }
}
