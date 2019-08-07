using Flying;
using UnityEngine;

/**
 * Work-around to enable multiple colliders on the same object. 
 * See: https://stackoverflow.com/questions/30132866/unity-multiple-collider-on-same-object
 */
public class ColliderPropagator : MonoBehaviour
{
    private Ship ship;

    void Awake()
    {
        ship = GetComponentInParent<Ship>();
    }

    void OnTriggerEnter(Collider other)
    {
        ship.OnTriggerEnter(other);
    }
}
