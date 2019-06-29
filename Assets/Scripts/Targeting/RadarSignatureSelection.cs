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

        void Start()
        {
            lineRenderer.useWorldSpace = false;
            lineRenderer.loop = true;
            lineRenderer.enabled = false;

            /* lineRenderer.SetPosition(0, new Vector3(-1.5f, 1.5f, 0));
             lineRenderer.SetPosition(1, new Vector3(1.5f, 1.5f, 0));
             lineRenderer.SetPosition(2, new Vector3(1.5f, -1.5f, 0));
             lineRenderer.SetPosition(3, new Vector3(-1.5f, -1.5f, 0));*/
             /*
            lineRenderer.SetPosition(0, new Vector3(-1.5f, 1.5f, -1.5f));
            lineRenderer.SetPosition(1, new Vector3(1.5f, 1.5f, -1.5f));
            lineRenderer.SetPosition(2, new Vector3(1.5f, -1.5f, -1.5f));
            lineRenderer.SetPosition(3, new Vector3(-1.5f, -1.5f, -1.5f));

            lineRenderer.SetPosition(4, new Vector3(-1.5f, -1.5f, 1.5f));
            lineRenderer.SetPosition(5, new Vector3(1.5f, -1.5f, 1.5f));
            lineRenderer.SetPosition(6, new Vector3(1.5f, 1.5f, 1.5f));
            lineRenderer.SetPosition(7, new Vector3(-1.5f, 1.5f, 1.5f));*/
        }

        void OnTargetted(bool targetted)
        {
            lineRenderer.enabled = targetted;
        }
    }
}
