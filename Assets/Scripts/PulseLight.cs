using System.Collections;
using UnityEngine;

/**
 * Pulses a light by modifying its intensity.
 */ 
[RequireComponent(typeof(Light))]
public class PulseLight : MonoBehaviour
{    
    public float waitTime = 0.1f;

    private bool pulsingUp = true;
    private new Light light;
    
    public void Awake()
    {
        light = GetComponent<Light>();
    }

    void Start()
    {
        light.intensity = 0;
        StartCoroutine("Pulse");
    }

    private IEnumerator Pulse() {
        while (true)
        {
            for (int i = 0; i < 10; i++)
            {
                if (pulsingUp)
                {
                    light.intensity += 0.1f;
                }
                else {
                    light.intensity -= 0.1f;
                }
                yield return new WaitForSeconds(waitTime);
            }
            pulsingUp = !pulsingUp;
        }
    }
}

