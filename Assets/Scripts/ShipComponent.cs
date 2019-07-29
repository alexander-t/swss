using Core;
using UnityEngine;

public class ShipComponent : MonoBehaviour
{
    public int hullPoints = 10;
    public GameObject visualComponent;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Constants.Tag_LaserBeam))
        {
            other.gameObject.SetActive(false);
            Destroy(other.gameObject);

            hullPoints = Mathf.Max(hullPoints - 10, 0);

            if (hullPoints <= 0)
            {
                visualComponent.SetActive(false);
            }
        }
    }
}
