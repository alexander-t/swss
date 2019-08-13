using UnityEngine;
using UnityEngine.UI;

public class PowerDistributionSystem : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField]
    private Image display;
    [SerializeField]
    private Sprite maxLaserSprite;
    [SerializeField]
    private Sprite neutralDistributionSprite;
    [SerializeField]
    private Sprite maxEngineSprite;
#pragma warning restore 0649

    private int enginePower = 1;
    private int laserPower = 1;

    public float EnginePower { get => GetMultiplier(enginePower); }
    public float LaserPower { get => GetMultiplier(laserPower); }

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
        }
        else if (laserPower == 2)
        {
            display.sprite = maxLaserSprite;
        }
    }

    private float GetMultiplier(int v)
    {
        switch (v)
        {
            case 0:
                return 0.5f;
            case 2:
                return 1f;
            default:
                return 0.75f;
        }
    }
}


