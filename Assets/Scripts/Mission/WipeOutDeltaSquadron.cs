using UnityEngine;

public class WipeOutDeltaSquadron : MonoBehaviour
{
    private GameObject xWingDelta1;
    private GameObject xWingDelta2;
    private GameObject xWingDelta3;

    void Awake()
    {
        xWingDelta1 = GameObject.Find("X-wing Delta 1");
        xWingDelta1 = GameObject.Find("X-wing Delta 2");
        xWingDelta3 = GameObject.Find("X-wing Delta 3");

        EventManager.onShipDestroyed += OnShipDestroyed;
    }

    void Start()
    {
        Cursor.visible = false;
    }

    private void OnShipDestroyed(string name)
    {
        Debug.Log(name);
    }
}
