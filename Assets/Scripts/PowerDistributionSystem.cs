using UnityEngine;
using UnityEngine.UI;

public class PowerDistributionSystem : MonoBehaviour
{
    public Image display;
    public Sprite maxLaserSprite;
    public Sprite neutralDistributionSprite;
    public Sprite maxEngineSprite;

    private int enginePower = 1;
    private int laserPower = 1;

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.F9))
        {
            laserPower = (laserPower + 1) % 3;
            enginePower = 2 - laserPower;
        }
        if (enginePower == 1 && laserPower == 1)
        {
            display.sprite = neutralDistributionSprite;
        }
        else if (enginePower == 2)
        {
            display.sprite = maxEngineSprite;
            Debug.Log("Engine");
        }
        else if (laserPower == 2)
        {
            display.sprite = maxLaserSprite;
            Debug.Log("Laser");
        }
    }
}


