using UnityEngine;

namespace Firing
{
    /**
     * This class is aimed to pool laser beams eventually, but right now it's just en entry point to firing. 
     */
    public class BeamPool : MonoBehaviour
    {
        public GameObject greenLaserBeamPrefab;
        public GameObject orangeLaserBeamPrefab;

        public void FireAt(GameObject owner, Transform gun, Vector3 target, LaserColor laserColor = LaserColor.Green)
        {
            GameObject go = (laserColor == LaserColor.Green)
                ? Instantiate(greenLaserBeamPrefab, gun.position, gun.rotation) as GameObject
                : Instantiate(greenLaserBeamPrefab, gun.position, gun.rotation) as GameObject;
            Beam beam = go.GetComponent<Beam>();
            beam.target = target;
            beam.owner = owner;
        }
    }
}
