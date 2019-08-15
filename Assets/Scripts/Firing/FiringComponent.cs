using Core;
using UnityEngine;

namespace Firing
{
    /**
     * A ship component that can fire.
     */
    public class FiringComponent : MonoBehaviour
    {
        private GameObject target;
        public GameObject muzzle;
        public AudioSource audioSource;
        [Space(10)]
        public float rateOfFire = 5;
        public float maxRange = 500;

        private BeamPool beamPool;
        private float nextFireTime;

        void Awake()
        {
            beamPool = GameObject.Find(Constants.BeamPool).GetComponent<BeamPool>();
        }

        void Update()
        {
            if (target != null)
            {
                muzzle.transform.LookAt(target.transform.position);
                if (Time.time >= nextFireTime && IsTargetWithinKillRange(muzzle.transform.position, target.transform.position))
                {
                    GameObject owningShip = GameObjects.GetParentShip(gameObject);
                    beamPool.FireAt(owningShip, muzzle.transform, target.transform.position, Beam.DefaultIntensity, maxRange, LaserColor.Orange);
                    audioSource.Play();

                    // Add a random delay so that two adjacent guns don't fire at the same time
                    nextFireTime = Time.time + rateOfFire + Random.Range(0.2f, 1);

                }
                Debug.DrawLine(muzzle.transform.position, target.transform.position, Color.red, 0.1f);
            }
        }

        private bool IsTargetWithinKillRange(Vector3 attackerPosition, Vector3 targetPosition)
        {
            return Vector3.Distance(attackerPosition, targetPosition) <= maxRange;
        }

        void OnTriggerEnter(Collider other)
        {
            if (GameObjects.IsBeam(other))
            {
                return;
            }

            if (GameObjects.IsPlayer(other.gameObject))
            {
                target = other.gameObject;
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (GameObjects.IsBeam(other))
            {
                return;
            }

            if (GameObjects.IsPlayer(other.gameObject))
            {
                target = null;
            }
        }
    }
}
