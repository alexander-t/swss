using Core;
using UnityEngine;

/**
 * Models a ship component, most probably a gun, that can be destroyed. It makes sense to have an independent component, 
 * because the hit surface may be larger than that of a gun's barrel, which improves playability.
 * The component has its own hitpoints, and when it gets destroyed an part of a model (visual component) can be set inactive.
 */ 
public class DestructableComponent : MonoBehaviour
{
    public int hullPoints = 10;

    [Tooltip("Part of the model that can be made inactive if destroyed.")]
    public GameObject visualComponent;
    [Tooltip("Component that holds the fire control and must be made inactive if this component is destroyed.")]
    public GameObject firingComponent;

    void OnTriggerEnter(Collider other)
    {
        if (GameObjects.IsBeam(other))
        {
            other.gameObject.SetActive(false);
            Destroy(other.gameObject);

            hullPoints = Mathf.Max(hullPoints - 10, 0);

            if (hullPoints <= 0)
            {
                if (visualComponent != null)
                {
                    visualComponent.SetActive(false);
                }

                if (firingComponent != null)
                {
                    firingComponent.SetActive(false);
                }
            }
        }
    }
}
