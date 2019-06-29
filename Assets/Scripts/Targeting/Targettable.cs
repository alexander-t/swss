namespace Targeting
{
    /**
     * An uniform interface that mathes the values displayed in the targeting computer.
     */ 
    public interface Targettable
    {
        string Name
        {
            get;
        }

        int HullPoints
        {
            get;
        }

        int ShieldPoints
        {
            get;
        }

        ShipFaction ShipFaction {
            get;
        }
    }
}
