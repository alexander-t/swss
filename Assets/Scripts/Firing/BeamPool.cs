using System.Collections.Generic;
using UnityEngine;

namespace Firing
{
    /**
     * A non-expandable pool for beams.
     */
    public class BeamPool : MonoBehaviour
    {
        public GameObject greenLaserBeamPrefab;
        public GameObject orangeLaserBeamPrefab;

        private const int BeamsToPreallocate = 25;
        private List<GameObject> greenBeams = new List<GameObject>(BeamsToPreallocate);
        private List<GameObject> orangeBeams = new List<GameObject>(BeamsToPreallocate);

        void Awake()
        {
            // Make sure that the beams aren't parented to some strange transform
            transform.position = Vector3.zero;

            GameObject go;
            for (int i = 0; i < BeamsToPreallocate; i++)
            {
                go = Instantiate(greenLaserBeamPrefab, Vector3.zero, Quaternion.identity, transform) as GameObject;
                go.SetActive(false);
                greenBeams.Add(go);

                go = Instantiate(orangeLaserBeamPrefab, Vector3.zero, Quaternion.identity, transform) as GameObject;
                go.SetActive(false);
                orangeBeams.Add(go);
            }
        }

        public void FireAt(GameObject owner, Transform gun, Vector3 target, LaserColor laserColor) {
            FireAt(owner, gun, target, 1.0f, laserColor);
        }

        public void FireAt(GameObject owner, Transform gun, Vector3 target, float initialIntensity = 1.0f, LaserColor laserColor = LaserColor.Green)
        {
            GameObject go = null;
            if (laserColor == LaserColor.Green)
            {
                go = FindUnusedBeam(greenBeams);
            }
            else {
                go = FindUnusedBeam(orangeBeams);
            }

            if (go == null)
            {
                Debug.Log("No more objects in pool.");
                return;
            }
            
            Beam beam = go.GetComponent<Beam>();
            beam.ShootAt(owner, target, initialIntensity, gun.position, gun.rotation);
            go.SetActive(true);
        }

        private GameObject FindUnusedBeam(List<GameObject> beams) {
            for (int i = 0; i < BeamsToPreallocate; i++)
            {
                if (!beams[i].activeInHierarchy)
                {
                    return beams[i];
                }
            }
            return null;
        }

        public void Reclaim(Beam beam)
        {
            beam.gameObject.SetActive(false);
        }
    }
}
