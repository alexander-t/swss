using Targeting;
using UnityEngine;

public class Ship : MonoBehaviour, Targettable
{
    #pragma warning disable 0649
    [SerializeField]
    private ShipData shipData;
    #pragma warning restore 0649

    // Bookkeeping 
    private GameObject mission;
    private GameObject targetingComputer;
    private bool isDestroyed = false;

    // Targettable
    private int hullPoints;
    private int shieldPoints;

    #region Properties
    public string Name
    {
        get => name;
    }

    public int HullPoints
    {
        get => hullPoints;
    }

    public int ShieldPoints
    {
        get => shieldPoints;
    }
    #endregion

    void Start() {
        hullPoints = shipData.HullPoints;
        shieldPoints = shipData.ShieldPoints;
        mission = GameObject.Find("Mission");
        targetingComputer = GameObject.Find("Targeting Computer");
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "LaserBeam" && !isDestroyed)
        {
            Destroy(other.gameObject);

            hullPoints -= 10;
            //Debug.Log(hullPoints);
            targetingComputer.BroadcastMessage("OnEnemyHit");
            if (hullPoints <= 0)
            {
                isDestroyed = true;

                gameObject.BroadcastMessage("Explode");
                DestroyModel();

                mission.BroadcastMessage("OnEnemyDestroyed", gameObject.name);
                targetingComputer.BroadcastMessage("OnEnemyDestroyed", gameObject.name);

                // Destroy the outer object with a certain delay, because it holds the explosion audio source
                Destroy(gameObject, 5);

            }
        }
    }


    private void DestroyModel()
    {
        GameObject model = transform.Find("Model").gameObject;
        if (model != null)
        {
            Destroy(model, 0.1f);
        }
    }
}
