using UnityEngine;

[CreateAssetMenu(fileName = "New Ship Data", menuName = "Ship Data", order = 51)]
public class ShipData : ScriptableObject
{
#pragma warning disable 0649
    [SerializeField]
    [Tooltip("In RU")]
    private int hullPoints;
    
    [SerializeField]
    [Tooltip("In SBD")]
    private int shieldPoints;

    [SerializeField]
    [Tooltip("In kmph")]
    private float maxSpeed;

    [SerializeField]
    private int angularVelocity;

    [SerializeField]
    private ShipSize shipSize;

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

    public float MaxSpeed
    {
        get => maxSpeed;
    }

    public float AngularVelocity
    {
        get => angularVelocity;
    }

    public ShipSize Size
    {
        get => shipSize;
    }

    public ShipFaction ShipFaction {
        get => shipFaction;
    }
}
