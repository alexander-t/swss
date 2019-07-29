using Core;
using UnityEngine;
namespace Firing
{
    public class SingleGunController : MonoBehaviour
    {
        [Tooltip("If set to null, the Player object will be used.")]
        public GameObject target;
        public GameObject gun;
        public AudioSource audioSource;
        [Space(10)]
        public float rateOfFire = 3;
        public float attackRange = 100;

        private BeamPool beamPool;
        private float nextFireTime;

        void Awake()
        {
            beamPool = GameObject.Find(Constants.BeamPool).GetComponent<BeamPool>();
            if (target == null)
            {
                target = GameObject.Find(Constants.Player);
            }
        }

        void Update()
        {
            if (target != null && Vector3.Distance(gun.transform.position, target.transform.position) <= attackRange)
            {
                transform.LookAt(target.transform.position);
                if (Time.time >= nextFireTime)
                {
                    beamPool.FireAt(gameObject, gun.transform, target.transform);
                    audioSource.Play();
                    nextFireTime = Time.time + rateOfFire;
                }
            }
        }
    }
}


