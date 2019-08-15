using Core;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AttackAnUndefendedDepot : MonoBehaviour
{
    private string[] unimportantContents = { "Food supplies", "Tools", "Machine parts", "Droids", "Fuel" };
    private const string CriticalContents = "Weapons";
    private int containersToDestroy = 6;

    void Awake()
    {
        SeedContainerGroupWithWeapons("Chi");
        SeedContainerGroupWithWeapons("Phi");
        SeedContainerGroupWithWeapons("Psi");
        SeedContainerGroupWithWeapons("Upsilon");

        EventManager.onShipDestroyed += OnShipDestroyed;
    }

    void Start()
    {
        Cursor.visible = false;
    }

    void OnDestroy()
    {
        EventManager.onShipDestroyed -= OnShipDestroyed;
    }

    private void OnShipDestroyed(string name)
    {
        Inspectable inspectableShip = GameObject.Find(name).GetComponent<Inspectable>();
        if (inspectableShip != null)
        {
            string containerContents = inspectableShip.GetComponent<Inspectable>().contents;
            if (CriticalContents == containerContents)
            {
                MissionEndData.losingReason = "You destroyed a container with weapons";
                MissionEndData.missionTime = Time.timeSinceLevelLoad;
                SceneManager.LoadScene(Constants.Scene_MissionFailed);
            }
            else
            {
                if (--containersToDestroy == 0)
                {
                    MissionEndData.missionTime = Time.timeSinceLevelLoad;
                    SceneManager.LoadScene(Constants.Scene_MissionWon);
                }
            }
        }
    }

    private void SeedContainerGroupWithWeapons(string groupName)
    {
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
