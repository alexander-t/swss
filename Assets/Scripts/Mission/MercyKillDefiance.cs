using Core;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MercyKillDefiance : MonoBehaviour
{
    void Awake()
    {
        EventManager.onShipDestroyed += OnShipDestroyed;
    }

    void Start()
    {
        Cursor.visible = false;
    }

    private void OnShipDestroyed(string name)
    {
        if (name == "Defiance")
        {
            MissionEndData.missionTime = Time.timeSinceLevelLoad;
            SceneManager.LoadScene(Constants.Scene_MissionWon);
        }
    }
}
