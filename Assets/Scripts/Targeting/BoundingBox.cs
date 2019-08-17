using Core;
using UnityEngine;

namespace Targeting
{
    public class BoundingBox : MonoBehaviour
    {
        [Tooltip("Prefab that contains a Line Renderer component setup so that it will produce a nice bounding box")]
        public GameObject boundingBoxPrefab;

        private LineRenderer lineRenderer;
        private Transform playerTransform;
        private float redrawTime;

        void Awake()
        {
            playerTransform = GameObject.Find(Constants.Player).transform;
            lineRenderer = boundingBoxPrefab.GetComponent<LineRenderer>();

            lineRenderer.useWorldSpace = false;
            lineRenderer.loop = true;
            lineRenderer.enabled = false;

            lineRenderer.SetPosition(0, new Vector3(-3, 3, 0));
            lineRenderer.SetPosition(1, new Vector3(3, 3, 0));
            lineRenderer.SetPosition(2, new Vector3(3, -3, 0));
            lineRenderer.SetPosition(3, new Vector3(-3, -3, 0));
        }

        void Update()
        {
            lineRenderer.transform.rotation = playerTransform.rotation;
            if (Time.time >= redrawTime)
            {
                float distanceCoefficient = Mathf.Clamp(Vector3.Distance(playerTransform.position, lineRenderer.transform.position), 0, 1000) / 1000;
                lineRenderer.transform.localScale = Vector3.one * Mathf.Lerp(0.75f, 12.5f, distanceCoefficient);
                lineRenderer.widthMultiplier = Mathf.Lerp(0.1f, 2f, distanceCoefficient);
                redrawTime = Time.time + 0.15f;
            }
        }

        public void OnTargetted(bool active)
        {
            lineRenderer.enabled = active;
        }
    }
}
