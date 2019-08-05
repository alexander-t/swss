using Core;
using Firing;
using UnityEngine;

/**
 * Models a ship component, most probably a gun, that can be destroyed. It makes sense to have an independent component, 
 * because the hit surface may be larger than that of a gun's barrel, which improves playability.
 * The component has its own hitpoints, and when it gets destroyed and part of a model (visual component) can be set inactive.
 */
public class DestructableComponent : MonoBehaviour
{
    public int hullPoints = 10;

    [Tooltip("Part of the model that can be made inactive if destroyed.")]
    public GameObject visualComponent;
    [Tooltip("Component that holds the fire control and must be made inactive if this component is destroyed.")]
    public GameObject firingComponent;
    [Tooltip("Prefab to use for explosion.")]
    public ParticleSystem explosionPrefab;
    [Tooltip("Used to tweak the exact point of the explosion.")]
    public Vector3 explosionOffset;

    private bool isAlive = true;

    void OnTriggerEnter(Collider other)
    {
        if (GameObjects.IsBeam(other) && isAlive)
        {
            Beam beam = other.GetComponentInParent<Beam>();
            beam.Destroy();

            hullPoints = Mathf.Max(hullPoints - 10, 0);

            if (hullPoints <= 0)
            {
                isAlive = false;
                if (visualComponent != null)
                {
                    visualComponent.SetActive(false);
                    Explode();
                }

                if (firingComponent != null)
                {
                    firingComponent.SetActive(false);
                }
            }
        }
    }

    private void Explode()
    {
        if (explosionPrefab != null)
        {
            Vector3 offset = explosionOffset != null ? explosionOffset : Vector3.zero;
            ParticleSystem explosion = Instantiate(explosionPrefab, transform.position + offset, Quaternion.identity);
            Destroy(explosion.gameObject, explosionPrefab.main.duration);
        }
    }
}
