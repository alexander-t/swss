using UnityEngine;

public class Exploding : MonoBehaviour
{
    #pragma warning disable 0649
    [SerializeField]
    private ParticleSystem explosionPrefab;

    [SerializeField]
    private AudioSource explosionAudioSource;
    #pragma warning restore 0649

    public void Explode()
    {
        ParticleSystem explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        explosionAudioSource.Play();
        Destroy(explosion, explosion.main.duration);
    }
}
