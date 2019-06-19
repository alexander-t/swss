using UnityEngine;

public class CosmicDustGenerator : MonoBehaviour
{
    public ParticleSystem cosmicDustPrefab;

    void Start()
    {
        for (int i = 0; i < 100; i++) {
            Vector3 position = new Vector3(Random.Range(-100.0f, 100.0f), Random.Range(-100.0f, 100.0f), Random.Range(-100.0f, 100.0f));
            Instantiate(cosmicDustPrefab, position, Quaternion.identity);
        }
    }
}
