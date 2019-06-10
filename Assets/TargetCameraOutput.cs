using UnityEngine;
using UnityEngine.UI;

public class TargetCameraOutput : MonoBehaviour
{
    public RawImage cameraOutput;
    public GameObject player;
    public GameObject target;
    public Camera trackingCamera;

    void Start()
    {
        var outputDisplay = cameraOutput.GetComponent<RectTransform>();
        float centerX = Screen.width / 2;
        float width = Screen.width / 5;
        float height = Screen.height / 4.5f;
        outputDisplay.sizeDelta = new Vector2(width, height);
        outputDisplay.position = new Vector3(centerX, 10, 0);
    }

    void LateUpdate()
    {
        Vector3 cameraPosition = target.transform.position + new Vector3(20, 10, 20);
        trackingCamera.transform.position = cameraPosition;
        trackingCamera.transform.LookAt(target.transform.position, player.transform.up);
    }
}
