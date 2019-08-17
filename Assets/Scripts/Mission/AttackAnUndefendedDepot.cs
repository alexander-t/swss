using Core;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AttackAnUndefendedDepot : MonoBehaviour
{
    private string[] unimportantContents = { "Food supplies", "Tools", "Machine parts", "Droids", "Fuel" };
    private const string CriticalContents = "Weapons";
    private int containersToDestroy = 12;

    private PlayerMissionEnd playerMissionEnd;

    void Awake()
    {
        playerMissionEnd = GameObject.Find(Constants.Player).GetComponent<PlayerMissionEnd>();

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
                playerMissionEnd.FailMission("You destroyed a container with weapons");
            }
            else
            {
                if (--containersToDestroy == 0)
                {
                    playerMissionEnd.WinMission();
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
