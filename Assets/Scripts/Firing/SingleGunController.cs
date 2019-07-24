using Core;
using UnityEngine;
namespace Firing
{
    public class SingleGunController : MonoBehaviour
    {
        [Tooltip("If set to null, the Player object will be used.")]
        public GameObject target;
        public GameObject gun;
        public GameObject beamPrefab;
        public AudioSource audioSource;
        [Space(10)]
        public float rateOfFire = 3;
        public float attackRange = 100;

        private float nextFireTime;

        void Awake()
        {
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
                    GameObject go = Instantiate(beamPrefab, gun.transform.position, gun.transform.rotation) as GameObject;
                    Beam beam = go.GetComponent<Beam>();
                    beam.target = target.transform.position;
                    beam.owner = this.gameObject;
                    audioSource.Play();
                    nextFireTime = Time.time + rateOfFire;
                }
            }
        }
    }
}


