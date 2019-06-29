using UnityEngine;

namespace Targeting
{
    /**
     * Governs the selection of the radar dot corresponding to the current target.
     **/ 
    public class RadarSignatureSelection : MonoBehaviour
    {
#pragma warning disable 0649
        [SerializeField]
        LineRenderer lineRenderer;
#pragma warning restore 0649

        private Transform playerTransform;

        private void Awake()
        {
            playerTransform = GameObject.Find("Player").GetComponent<Transform>();
        }

        void Start()
        {
            lineRenderer.widthMultiplier = 0.45f;
            lineRenderer.enabled = false;
        }

        void Update()
        {
            lineRenderer.transform.rotation = playerTransform.rotation;
        }

        void OnTargetted(bool targetted)
        {
            lineRenderer.enabled = targetted;
        }
    }
}
