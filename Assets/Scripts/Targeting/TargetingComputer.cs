using System.Collections.Generic;
using System.Linq;

namespace Targeting
{
    public class TargetingComputer 
    {
        private static TargetingComputer instance;
        private List<Targettable> targets = new List<Targettable>();
        private int currentTargetIndex = 0;

        private TargetingComputer() { }

        public static TargetingComputer Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new TargetingComputer();
                }
                return instance;
            }
        }

        public Targettable GetCurrentTarget()
        {
            if (targets.Count == 0)
            {
                return null;
            }
            return targets.ElementAt(currentTargetIndex);
        }

        public void AddTarget(Targettable target)
        {
            targets.Add(target);
        }

        public void RemoveTargetByName(string name)
        {
            for (int i = 0; i < targets.Count; i++)
            {
                if (targets.ElementAt(i).Name == name)
                {
                    targets.RemoveAt(i);

                    if (currentTargetIndex == targets.Count)
                    {

                        // Cycle if active target is the last one to be removed
                        currentTargetIndex %= targets.Count;
                    }
                    break;
                }
            }
        }

        public void NextTarget()
        {
            currentTargetIndex = ++currentTargetIndex % targets.Count;
        }
    }
}
