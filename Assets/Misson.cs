using UnityEngine;

public class Misson : MonoBehaviour
{
    public GameObject[] enemyShips;

    void OnEnemyDestroyed()
    {
        Vector3 spawnPoint = new Vector3(-100, 0, 100);
        var xWingPrefab = enemyShips[0];
        Instantiate(xWingPrefab, transform.position, Quaternion.identity);
    }
}
