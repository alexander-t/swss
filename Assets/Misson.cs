using Targeting;
using Flying;
using UnityEngine;

public class Misson : MonoBehaviour
{
    public GameObject[] enemyShips;

    private readonly Vector3 SpawnPoint = new Vector3(100, 0, 100);
    private int rookieIndex = 1;

    void Start()
    {
        Cursor.visible = false;
        foreach (var buoyGO in GameObject.FindGameObjectsWithTag("Buoy"))
        {
            TargetingComputer.Instance.AddTarget(buoyGO.GetComponent<Ship>());
        }

        SpawnXWing();
    }

    private void SpawnXWing()
    {
        var xWingPrefab = enemyShips[0];

        GameObject xWing = Instantiate(xWingPrefab, SpawnPoint, Quaternion.identity);
        xWing.name = "Rookie " + rookieIndex++;
        AIPilot aiPilot = xWing.GetComponent<AIPilot>();
        aiPilot.waypoints = new Vector3[] { new Vector3(-100, 0, 100), new Vector3(-100, 0, -100), new Vector3(100, 0, -100), new Vector3(100, 0, 100) };
        aiPilot.velocity = 30;

        TargetingComputer.Instance.AddTarget(xWing.GetComponent<Ship>());
    }

    #region Events
    public void OnEnemyDestroyed(string name)
    {
        TargetingComputer.Instance.RemoveTargetByName(name);
        GameObject.Find("Targeting Computer").SendMessage("UpdateDisplay");
        SpawnXWing();
    }
    #endregion

}
