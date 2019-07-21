using UnityEngine;
using Core;
/*
* Spawner that drags a sphere along a Bezier curve and spawns objects within that sphere's radius.
*/
namespace Environment
{
    public class BezierSpawner : MonoBehaviour
    {
        public Transform p0;
        public Transform p1;
        public Transform p2;
        public Transform p3;
        public Axis axis = Axis.X_Y;
        public float radius = 1;
        public float step = 0.01f;
        public float objectsPerStep = 1;
        [Space(10)]
        public GameObject[] spawnPrefabs;
        public float minScale = 1;
        public float maxScale = 1;
        public bool applyRandomRotation = true;
        [Space(10)]
        public bool drawAllGizmos = false;

        void Start()
        {
            for (float t = 0; t <= 1; t += step)
            {
                Vector3 center = CalculatePosition(t, p0.position, p1.position, p2.position, p3.position, axis);
                for (int i = 0; i < objectsPerStep; i++)
                {
                    Vector3 spawnPosition = center + Random.insideUnitSphere * radius;
                    int prefabIndex = Random.Range(0, spawnPrefabs.Length);

                    GameObject newObject = Instantiate(spawnPrefabs[prefabIndex], spawnPosition, Quaternion.identity, this.transform) as GameObject;
                    newObject.transform.localScale = Vector3.one * Random.Range(minScale, maxScale);
                    if (applyRandomRotation)
                    {
                        newObject.transform.Rotate(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
                    }
                }
            }
        }

        void OnDrawGizmos()
        {
            for (float t = 0; t <= 1; t += step)
            {
                Gizmos.color = Color.white;
                Gizmos.DrawSphere(CalculatePosition(t, p0.position, p1.position, p2.position, p3.position, axis), 0.5f);

                // Save the poor editor
                if (drawAllGizmos)
                {
                    Vector3 center = CalculatePosition(t, p0.position, p1.position, p2.position, p3.position, axis);
                    for (int i = 0; i < objectsPerStep; i++)
                    {
                        Vector3 spawnPoint = center + Random.insideUnitSphere * radius;
                        Gizmos.color = Color.blue;
                        Gizmos.DrawSphere(spawnPoint, Random.Range(minScale, maxScale));
                    }
                }
            }
        }

        private Vector3 CalculatePosition(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, Axis axis)
        {
            if (axis == Axis.X_Z)
            {
                float x = CubicBezierPoint(t, p0.x, p1.x, p2.x, p3.x);
                float z = CubicBezierPoint(t, p0.z, p1.z, p2.z, p3.z);
                return new Vector3(x, 0, z);
            }
            else if (axis == Axis.Y_Z)
            {
                float y = CubicBezierPoint(t, p0.y, p1.y, p2.y, p3.y);
                float z = CubicBezierPoint(t, p0.z, p1.z, p2.z, p3.z);
                return new Vector3(0, y, z);
            }
            else
            {
                float x = CubicBezierPoint(t, p0.x, p1.x, p2.x, p3.x);
                float y = CubicBezierPoint(t, p0.y, p1.y, p2.y, p3.y);
                return new Vector3(x, y, 0);
            }

        }

        private float CubicBezierPoint(float t, float p0, float p1, float p2, float p3)
        {
            return Mathf.Pow(1 - t, 3) * p0 + 3 * Mathf.Pow(1 - t, 2) * t * p1 + 3 * (1 - t) * Mathf.Pow(t, 2) * p2 + Mathf.Pow(t, 3) * p3;
        }
    }
}



