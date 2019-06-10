using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class TargetingComputerSizer : MonoBehaviour
    {
        public RawImage outputImage;
        
        void Start()
        {
            var outputRectTransform = outputImage.GetComponent<RectTransform>();
            float centerX = Screen.width / 2;
            float width = Screen.width / 5;
            float height = Screen.height / 4.5f;
            outputRectTransform.sizeDelta = new Vector2(width, height);
        }
    }
}
