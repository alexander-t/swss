using UnityEngine;

namespace Firing
{
    public class Beam : MonoBehaviour
    {
        public Transform start;
        public Transform end;

        [Space(7)]        
        public Vector3 target;
        public GameObject owner;

        private const float Velocity = 500f;
        public static float MaxRange = 250f;

        private Vector3 startPosition;
        private LineRenderer lineRenderer;

        void Awake()
        {
            lineRenderer = GetComponent<LineRenderer>();
        }

        void Start()
        {
            transform.LookAt(target);
            startPosition = transform.position;
        }

        void Update()
        {
            transform.Translate(0, 0, Velocity * Time.deltaTime);
            if ((transform.position - startPosition).magnitude > MaxRange)
            {
                Destroy(gameObject);
            }
            lineRenderer.SetPosition(0, start.position);
            lineRenderer.SetPosition(1, end.position);
        }
    }
}