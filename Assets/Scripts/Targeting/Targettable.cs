namespace Targeting
{
    public abstract class Targettable
    {
        protected string name;
        protected int hullPoints;

        public Targettable(string name, int hullPoints)
        {
            this.name = name;
            this.hullPoints = hullPoints;
        }

        public string Name
        {
            get { return name; }
        }

        public int HullPoints
        {
            get { return hullPoints; }
        }
    }
}
