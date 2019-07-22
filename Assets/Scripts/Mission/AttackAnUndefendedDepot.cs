using Core;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AttackAnUndefendedDepot : MonoBehaviour
{
    private string[] unimportantContents = { "Food supplies", "Tools", "Machine parts" };
    private const string CriticalContents = "Weapons";

    void Awake()
    {
        EventManager.onShipInspected += OnShipInspected;
        EventManager.onShipDestroyed += OnShipDestroyed;
        SeedContainerGroupWithWeapons("Chi");
    }

    void Start()
    {
        Cursor.visible = false;
    }

    private void OnShipInspected(string name)
    {
        GameObject container = GameObject.Find(name);
        string containerContents = container.GetComponent<Inspectable>().contents;
    }

    private void OnShipDestroyed(string name) {
        Inspectable inspectableShip = GameObject.Find(name).GetComponent<Inspectable>();
        if (inspectableShip != null)
        {
            string containerContents = inspectableShip.GetComponent<Inspectable>().contents;
            if (CriticalContents == containerContents)
            {
                MissionEndData.losingReason = "You destroyed a container with weapons";
                SceneManager.LoadScene(Constants.Scene_MissionFailed);
            }
        }
    }

    private void SeedContainerGroupWithWeapons(string groupName) {
        int weaponIndex = Random.Range(0, 4);

        for (int i = 0; i < 4; i++)
        {
            Inspectable container = GameObject.Find(groupName + " " + (i + 1)).GetComponent<Inspectable>();
            if (i == weaponIndex)
            {
                container.contents = CriticalContents;
            }
            else
            {
                container.contents = unimportantContents[Random.Range(0, unimportantContents.Length)];
            }
        }
    }
}
