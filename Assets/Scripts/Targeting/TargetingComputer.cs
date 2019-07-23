using System.Collections.Generic;
using System.Linq;

namespace Targeting
{
    public class TargetingComputer 
    {
        private List<Targettable> targets = new List<Targettable>();
        private int currentTargetIndex = 0;

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

                        if (targets.Count > 0)
                        {
                            // Cycle if active target is the last one to be removed.
                            currentTargetIndex %= targets.Count;
                        }
                        else
                        {
                            // Do nothing if the last target was removed and no other targets remain.
                            return;
                        }
                    }
                    break;
                }
            }
        }

        public void NextTarget()
        {
            if (targets.Count > 0)
            {
                currentTargetIndex = ++currentTargetIndex % targets.Count;
            }
        }

        public void PreviousTarget()
        {
            if (targets.Count > 0)
            {
                currentTargetIndex = (--currentTargetIndex % targets.Count + targets.Count) % targets.Count;
            }
        }
    }
}
