using Core;
using UnityEngine;

namespace Firing
{
    public class Beam : MonoBehaviour
    {
        public Transform start;
        public Transform end;

        private const float Velocity = 500f;
        public static float MaxRange = 250f;

        private Vector3 startPosition;
        private LineRenderer lineRenderer;
        private BeamPool beamPool;


        public GameObject Owner { get; private set; }

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

            if ((transform.position - startPosition).magnitude > MaxRange)
            {
                beamPool.Reclaim(this);
            }
        }


        public void ShootAt(GameObject owner, Vector3 target, Vector3 startPosition, Quaternion rotation)
        {
            Owner = owner;
            transform.position = this.startPosition = startPosition;
            transform.rotation = rotation;
            transform.LookAt(target);
        }

        public void Destroy()
        {
            beamPool.Reclaim(this);
        }
    }
}