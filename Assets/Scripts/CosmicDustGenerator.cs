using UnityEngine;

/**
 * Generates a number of particle systems and then moves them around so that they always
 * remain close to the player.
 */
public class CosmicDustGenerator : MonoBehaviour
{
    public ParticleSystem cosmicDustPrefab;
    public GameObject player;
    public int numberOfClounds = 10;

    private ParticleSystem[] dustClouds;

    void Start()
    {
        dustClouds = new ParticleSystem[numberOfClounds];
        for (int i = 0; i < numberOfClounds; i++) {
            dustClouds[i] = Instantiate(cosmicDustPrefab, PointCloseToPlayer(), Quaternion.identity, this.transform);
        }
    }

    void Update()
    {
        for (int i = 0; i < numberOfClounds; i++)
        {
            if (Random.Range(0, 100) < 0.001f) {
                dustClouds[i].transform.position = PointCloseToPlayer();
            }
        }
    }

    private Vector3 PointCloseToPlayer() {
        Vector3 position = player.transform.position;
        position.x += Random.Range(-100.0f, 100.0f);
        position.y += Random.Range(-100.0f, 100.0f);
        position.z += Random.Range(-100.0f, 100.0f);
        return position;
    }
}
