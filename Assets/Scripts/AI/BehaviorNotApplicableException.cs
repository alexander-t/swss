using System;

namespace AI
{
    /**
     * Thrown to indicate that a certain behavior cannot be applied.
     */
    public class BehaviorNotApplicableException : Exception
    {
        public BehaviorChangeReason Reason { get; private set; }

        public BehaviorNotApplicableException(BehaviorChangeReason reason)
        {
            Reason = reason;
        }
    }
}
