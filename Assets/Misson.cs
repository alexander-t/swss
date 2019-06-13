using Targeting;
using UnityEngine;

public class Misson : MonoBehaviour
{
    public GameObject[] enemyShips;

    private Vector3 SpawnPoint = new Vector3(100, 0, 100);
    private int rookieIndex = 1;

    void Start()
    {
        Cursor.visible = false;
        foreach (var buoyGO in GameObject.FindGameObjectsWithTag("Buoy")) {
            TargetingComputer.Instance.AddTarget(new Buoy(buoyGO.name));
        }
        
        SpawnXWing();
    }

    private void SpawnXWing() {
        var xWingPrefab = enemyShips[0];
        GameObject xWing = Instantiate(xWingPrefab, SpawnPoint, Quaternion.identity);
        xWing.name = "Rookie " + rookieIndex++;
        TargetingComputer.Instance.AddTarget(new XWing(xWing.name));
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
