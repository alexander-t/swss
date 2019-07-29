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

        public void FireAt(GameObject owner, Transform gun, Transform target)
        {
            GameObject go = Instantiate(orangeLaserBeamPrefab, gun.position, gun.rotation) as GameObject;
            Beam beam = go.GetComponent<Beam>();
            beam.target = target.position;
            beam.owner = owner;
        }
    }
}
