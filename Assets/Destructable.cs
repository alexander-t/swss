using UnityEngine;

public class Destructable : MonoBehaviour
{
    public int hp;
    public ParticleSystem explosionPrefab;
    public AudioSource explosionAudioSource;

    private GameObject mission;
    private GameObject targetingComputer;

    private bool isDestroyed = false;

    private void Start()
    {
        mission = GameObject.Find("Mission");
        targetingComputer = GameObject.Find("Targeting Computer");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "LaserBeam" && !isDestroyed)
        {
            Destroy(other.gameObject);

            hp -= 10;
            if (hp <= 0)
            {
                isDestroyed = true;

                Explode();
                DestroyModel();

                mission.BroadcastMessage("OnEnemyDestroyed", gameObject.name);
                targetingComputer.BroadcastMessage("OnEnemyDestroyed", gameObject.name);

                // Destroy the outer object with a certain delay, because it holds the explosion audio source
                Destroy(gameObject, 5);
                
            }
        }
    }

    private void Explode() {
        ParticleSystem explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        explosionAudioSource.Play();
        Destroy(explosion, explosion.main.duration);
    }

    private void DestroyModel() {
        GameObject model = transform.Find("Model").gameObject;
        if (model != null)
        {
            Destroy(model, 0.1f);
        }
    }
}
