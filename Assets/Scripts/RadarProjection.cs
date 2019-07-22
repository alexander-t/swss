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

    private Transform playerTransform;
    private Vector3 direction;

    void Awake()
    {
        playerTransform = GameObject.Find(Constants.Player).transform;
    }

    void Update()
    {
        direction = (transform.position - playerTransform.position).normalized;
        radarSignatureObject.transform.position = playerTransform.position + direction * DistanceToRadar;
    }
}
