using UnityEngine;

public class Spawn : MonoBehaviour
{
    public GameObject shipPrefab;
    
    void Start()
    {
        Instantiate(shipPrefab, transform.position, Quaternion.identity);
    }
}
