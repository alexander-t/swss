using System.Linq;
using UnityEngine;

public class Destructable : MonoBehaviour
{
    public int hp;
    public ParticleSystem explosionPrefab;
    public AudioSource explosionAudioSource;

    private bool isDestroyed = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "LaserBeam" && !isDestroyed)
        {
            Destroy(other.gameObject);

            hp -= 10;
            if (hp <= 0)
            {
                isDestroyed = true;
                ParticleSystem explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
                explosionAudioSource.Play();
                float explosionDurtion = explosion.main.duration;
                GameObject model = transform.Find("Model").gameObject;
                if (model != null) { 
                    Destroy(model, 0.1f);
                }
                // Destroy the outer object with a certain delay, because it holds the explosion audio source
                Destroy(gameObject, 5);
                Destroy(explosion, explosionDurtion);
            }
        }
    }
}
