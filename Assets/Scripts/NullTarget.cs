namespace Targeting
{
    public class NullTarget : Targettable
    {
        public string Name {
            get => "";
        }

        public int HullPoints
        {
            get => 0;
        }

        public int ShieldPoints
        {
            get => 0;
        }
    }
}
