using UnityEngine;

public class Exploding : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField]
    private ParticleSystem explosionPrefab;
#pragma warning restore 0649

    void Awake()
    {
        EventManager.onShipDestroyed += Explode;    
    }

    void OnDestroy()
    {
        EventManager.onShipDestroyed -= Explode;
    }


    public void Explode(string name)
    {
        if (gameObject.name == name)
        {
            ParticleSystem explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            Destroy(explosion.gameObject, explosionPrefab.main.duration);
        }
    }
}
