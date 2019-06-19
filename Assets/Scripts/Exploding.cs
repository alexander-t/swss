using UnityEngine;

public class Exploding : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField]
    private ParticleSystem explosionPrefab;
#pragma warning restore 0649


    public void Explode()
    {
        ParticleSystem explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        Destroy(explosion.gameObject, explosionPrefab.main.duration);
    }
}
