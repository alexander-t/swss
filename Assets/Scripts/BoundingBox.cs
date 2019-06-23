using UnityEngine;

public class BoundingBox : MonoBehaviour
{
    public GameObject boundingBoxPrefab;

    private LineRenderer lineRenderer;
    private Transform playerTransform;

    void Start()
    {
        playerTransform = GameObject.Find("Player").GetComponent<Transform>();

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
        lineRenderer.transform.LookAt(playerTransform);
        lineRenderer.widthMultiplier = Mathf.Lerp(0.025f, 0.25f, Vector3.Distance(playerTransform.position, transform.position) / 100);
    }

    public void OnTargetted(bool active) {
        lineRenderer.enabled = active;
    }

}
