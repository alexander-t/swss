using UnityEngine;
using UnityEngine.UI;

public class TargetingComputerSizer : MonoBehaviour
{
    public RawImage outputImage;

    void Start()
    {
        var outputRectTransform = outputImage.GetComponent<RectTransform>();
        float centerX = Screen.width / 2;
        float width = Screen.width / 4.9f;
        float height = Screen.height / 4.2f;
        outputRectTransform.sizeDelta = new Vector2(width, height);
    }
}