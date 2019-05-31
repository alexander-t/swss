using UnityEngine;

public class Cockpit : MonoBehaviour
{
    public Texture2D crosshairImage;

    private float crosshairX;
    private float crosshairY;

    void Start()
    {
        crosshairX = (Screen.width / 2) - (crosshairImage.width / 2);
        crosshairY = (Screen.height / 2) - (crosshairImage.height / 2);
    }

    private void OnGUI()
    {
        GUI.DrawTexture(new Rect(crosshairX, crosshairY, crosshairImage.width, crosshairImage.height), crosshairImage);
    }
}
