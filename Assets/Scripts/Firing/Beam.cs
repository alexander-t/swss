using Core;
using UnityEngine;

namespace Firing
{
    public class Beam : MonoBehaviour
    {
#pragma warning disable 0649
        [SerializeField]
        private Transform start;

        [SerializeField]
        private Transform end;
#pragma warning restore 0649

        private const float Velocity = 500f;
        public const float DefaultMaxRange = 500f;
        public const float DefaultIntensity = 1.0f;
        public static float BaseDamage = 10;

        private Vector3 startPosition;
        private float maxRange = DefaultMaxRange;
        private float initialIntensity;

        private LineRenderer lineRenderer;
        private BeamPool beamPool;


        public GameObject Owner { get; private set; }

        /*
         * Used to compute damage. At long range, the beam's intensity diminishes
         */
        public float Intensity {
            get => Mathf.Lerp(initialIntensity, 0.25f, Vector3.Distance(transform.position, startPosition) / DefaultMaxRange);
        }

        void Awake()
        {
            lineRenderer = GetComponent<LineRenderer>();
            beamPool = GameObject.Find(Constants.BeamPool).GetComponent<BeamPool>();
        }

        void Update()
        {
            transform.Translate(0, 0, Velocity * Time.deltaTime);

            lineRenderer.SetPosition(0, start.position);
            lineRenderer.SetPosition(1, end.position);

            if ((transform.position - startPosition).magnitude > maxRange)
            {
                beamPool.Reclaim(this);
            }
        }


        public void ShootAt(GameObject owner, Vector3 target, Vector3 startPosition, Quaternion rotation, float initialIntensity = DefaultIntensity, float maxRange = DefaultMaxRange)
        {
            Owner = owner;
            this.initialIntensity = initialIntensity;
            this.maxRange = maxRange;
            transform.position = this.startPosition = startPosition;
            transform.rotation = rotation;
            transform.LookAt(target);
        }

        public void Destroy()
        {
            beamPool.Reclaim(this);
        }


        public static bool IsTargetInKillRange(Vector3 attackerPosition, Vector3 targetPosition)
        {
            return Vector3.Distance(attackerPosition, targetPosition) <= DefaultMaxRange;
        }
    }
}