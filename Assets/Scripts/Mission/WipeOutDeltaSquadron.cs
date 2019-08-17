using Core;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WipeOutDeltaSquadron : MonoBehaviour
{
    private List<string> shipsToKill = new List<string> { "X-wing Delta 1", "X-wing Delta 2", "X-wing Delta 3" };

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
        shipsToKill.Remove(name);
        if (shipsToKill.Count == 0)
        {
            GameObject.Find(Constants.Player).GetComponent<PlayerMissionEnd>().WinMission();
        } 
    }
}
