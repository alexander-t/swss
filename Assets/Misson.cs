using UnityEngine;

public class Misson : MonoBehaviour
{
    public GameObject[] enemyShips;

    void Start()
    {
        Cursor.visible = false;
    }

    void OnEnemyDestroyed()
    {
        Vector3 spawnPoint = new Vector3(-100, 0, 100);
        var xWingPrefab = enemyShips[0];
        Instantiate(xWingPrefab, spawnPoint, Quaternion.identity);
    }
}
