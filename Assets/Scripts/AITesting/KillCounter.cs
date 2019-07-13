using Flying;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillCounter : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }


    void OnNewTarget(Ship ship) {
        Debug.Log("OnNewTarget: " + ship.name);
    }

    void OnEnemyHit() {
        Debug.Log("Hit!");
    }

    void OnEnemyDestroyed(string enemyName)
    {
        Debug.Log("OnEnemyDestroyed: " + enemyName);
    }
}
