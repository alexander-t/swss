using UnityEngine;

[CreateAssetMenu(fileName = "New Ship Data", menuName = "Ship Data", order = 51)]
public class ShipData : ScriptableObject
{
#pragma warning disable 0649
    [SerializeField]
    private int hullPoints;

    [SerializeField]
    private int shieldPoints;

    [SerializeField]
    private float maxVelocity;

    [SerializeField]
    private int angularVelocity;

    [SerializeField]
    private ShipFaction shipFaction;
#pragma warning restore 0649

    public int HullPoints
    {
        get => hullPoints;
    }

    public int ShieldPoints
    {
        get => shieldPoints;
    }

    public float MaxVelocity
    {
        get => maxVelocity;
    }

    public float AngularVelocity
    {
        get => angularVelocity;
    }

    public ShipFaction ShipFaction {
        get => shipFaction;
    }
}
