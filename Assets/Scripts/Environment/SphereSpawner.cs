using UnityEngine;

namespace Environment
{
    /**
     * Spawns asteroids in an area bounded by a sphere.
     **/
    [RequireComponent(typeof(Transform))]
    public class SphereSpawner : MonoBehaviour
    {
        public GameObject[] asteroidPrefabs;
        public float radius = 10;
        public int asteroids = 10;
        public float minScale = 1;
        public float maxScale = 1;

        void Start()
        {
            for (int i = 0; i < asteroids; i++)
            {
                Vector3 position = transform.position + Random.insideUnitSphere * radius;
                float scale = Random.Range(minScale, maxScale);
                int prefabIndex = Random.Range(0, asteroidPrefabs.Length);

                GameObject asteroid = Instantiate(asteroidPrefabs[prefabIndex], position, Quaternion.identity, this.transform) as GameObject;
                asteroid.transform.localScale = new Vector3(scale, scale, scale);
                asteroid.transform.Rotate(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
            }
        }

        void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, radius);
            for (int i = 0; i < asteroids; i++)
            {
                Vector3 position = transform.position + Random.insideUnitSphere * radius;
                Gizmos.DrawSphere(position, Random.Range(minScale, maxScale));
            }
        }
    }
}