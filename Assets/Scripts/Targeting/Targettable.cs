namespace Targeting
{
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
    }
}
