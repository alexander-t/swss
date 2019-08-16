using Core;
using UnityEngine;
namespace Firing
{
    public class SingleGunController : MonoBehaviour
    {
        [Tooltip("If set to null, the Player object will be used.")]
        public GameObject target;
        public GameObject gunMuzzle;
        public AudioSource audioSource;
        [Space(10)]
        public float rateOfFire = 3;
        public float attackRange = 500;

        private readonly Vector3 rcOffset = new Vector3(0, 0, 1);

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
            if (target != null && Vector3.Distance(gunMuzzle.transform.position, target.transform.position) <= attackRange)
            {
                transform.LookAt(target.transform.position);
                if (Time.time >= nextFireTime)
                {
                    RaycastHit hit;
                    if (Physics.Raycast(gunMuzzle.transform.position, transform.forward, out hit, attackRange))
                    {
                        if (GameObjects.IsPlayer(hit.transform.gameObject))
                        {
                            beamPool.FireAt(gameObject, gunMuzzle.transform, target.transform.position, LaserColor.Orange);
                            audioSource.Play();
                            Debug.DrawLine(gunMuzzle.transform.position, hit.transform.position, Color.green, 0.1f);
                        }
                        else {
                            Debug.DrawLine(gunMuzzle.transform.position, hit.transform.position, Color.red, 0.1f);
                        }
                        nextFireTime = Time.time + rateOfFire;
                    }
                }
            }
        }
    }
}


