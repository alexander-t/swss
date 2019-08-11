using System;
using UnityEngine;

namespace AI
{
    /**
     * Thrown to indicate that a certain behavior cannot be applied.
     */
    public class BehaviorNotApplicableException : Exception
    {
        public BehaviorChangeReason Reason { get; private set; }
        public GameObject Target { get; private set; }

        public BehaviorNotApplicableException(BehaviorChangeReason reason) : this(reason, null) { }

        public BehaviorNotApplicableException(BehaviorChangeReason reason, GameObject target)
        {
            Reason = reason;
            Target = target;
        }
    }
}
