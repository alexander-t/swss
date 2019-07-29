using Core;
using UnityEngine;

/**
 * Creates a shield by fading in a material quickly and then fading it out more slowly.
 * To create a more interesting effect a light source is placed just outside the shield at the point of last impact.
 */ 
public class Shield : MonoBehaviour
{
    private const float duration = 0.5f;
    private const float MaxAlpha = 0.3f;
    private const float ImpactLightOffset = 3f;

    private Material material;
    private GameObject impactLight;
    private Color color;
    private bool hit;
    private float fadeTime;
    private Vector3 lastHitPosition;

    void Start()
    {
        material = GetComponent<Renderer>().material;
        impactLight = GameObject.Find(Constants.Shield_ImpactLight);
        impactLight.SetActive(false);

        color = material.color;
        color.a = 0;
        material.color = color;
    }

    void Update()
    {
        if (hit)
        {
            fadeTime += Time.deltaTime;
            if (fadeTime < duration / 5) // Fade in fast on hit...
            {
                color.a = Mathf.Lerp(0, MaxAlpha, fadeTime / duration);
            }
            else if (fadeTime < duration) // ... and fade out slower on cooldown
            {
                color.a = Mathf.Lerp(MaxAlpha, 0, fadeTime / duration);
                impactLight.SetActive(false);
            }
            else
            {
                hit = false;
                color.a = 0;
            }
            material.color = color;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (GameObjects.IsBeam(other))
        {
            lastHitPosition = other.transform.position;
            Vector3 impactLightDirection = (lastHitPosition - transform.position);
            impactLightDirection.Normalize();

            // Where does a ray hit the shield?
            Debug.DrawLine(transform.position, lastHitPosition + impactLightDirection * ImpactLightOffset, Color.red, 1f);
            impactLight.transform.position = lastHitPosition + impactLightDirection * ImpactLightOffset;
            impactLight.transform.LookAt(transform.position);
        }
    }

    void OnShieldImpact()
    {
        hit = true;
        fadeTime = 0;
        impactLight.SetActive(true);
    }
}
