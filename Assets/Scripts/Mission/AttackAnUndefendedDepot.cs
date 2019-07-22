using UnityEngine;

public class AttackAnUndefendedDepot : MonoBehaviour
{
    void Start()
    {
        Cursor.visible = false;
        EventManager.onShipInspected += ContainerInspected;
    }

    void ContainerInspected(string containerName)
    {
        Debug.Log("Container Inspected! " + containerName);
    }
}
