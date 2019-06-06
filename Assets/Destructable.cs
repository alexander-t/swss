using UnityEngine;

public class Destructable : MonoBehaviour
{
    public int hp;
    public ParticleSystem explosionPrefab;

    private bool isDestroyed = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "LaserBeam" && !isDestroyed)
        {
            Destroy(other.gameObject);

            hp -= 10;
            if (hp <= 0) {
                isDestroyed = true;
                ParticleSystem explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
                float explosionDurtion = explosion.main.duration;
                Destroy(gameObject, 0.1f);
                Destroy(explosion, explosionDurtion);
            }
        }
    }
}
