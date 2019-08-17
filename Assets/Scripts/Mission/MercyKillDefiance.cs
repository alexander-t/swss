using Core;
using UnityEngine;

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
            GameObject.Find(Constants.Player).GetComponent<PlayerMissionEnd>().WinMission();
        }
    }
}
