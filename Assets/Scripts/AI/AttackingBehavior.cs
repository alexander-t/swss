using Flying;
using UnityEngine;

namespace AI
{
    public class AttackingBehavior : Behavior
    {
        private const float TurnDampening = 0.025f;
        private const float FiringDistanceTreshhold = 3.0f;
        private const float CloseRangeAdjustmentTreshold = 100f;

        // Very X-wing-specific
        private readonly Vector3 RightGunOffset = new Vector3(5.86f, 1.64f, -3.2f);
        private readonly Vector3 LeftGunOffset = new Vector3(-5.86f, 1.64f, -3.2f);

        private Transform managedTransform;
        private GameObject managedShip;
        private Ship ship;
        private bool targetInKillZone;
        private bool closeRangeAdjustmentPerformed;

        private GameObject target;

        public AttackingBehavior(GameObject managedShip, GameObject target)
        {
            this.managedShip = managedShip;
            this.target = target;
            managedTransform = managedShip.transform;
            ship = managedShip.GetComponent<Ship>();
        }


        public void Commence()
        {

        }

        public void Turn()
        {
            if (target == null)
            {
                throw new BehaviorNotApplicableException();
            }

            if (Vector3.Distance(target.transform.position, managedTransform.position) < CloseRangeAdjustmentTreshold)
            {
                AdjustForCloseRange();
            }
            else
            {
                TurnTowards(target.transform.position);
            }
        }

        public void Attack()
        {
            if (target != null && (targetInKillZone || closeRangeAdjustmentPerformed))
            {
                managedShip.BroadcastMessage("OnFire");
            }
        }

        public string Describe()
        {
            return "Attacking " + target;
        }

        public void TargetEnteredKillzone(GameObject potentialTarget)
        {
            if (potentialTarget == target)
            {
                targetInKillZone = true;
            }
        }

        public void TargetLeftKillzone(GameObject potentialTarget)
        {
            if (potentialTarget == target)
            {
                targetInKillZone = false;
            }
        }

        private void TurnTowards(Vector3 target)
        {
            Vector3 targetDirection = target - managedTransform.position;
            Quaternion finalRotation = Quaternion.LookRotation(targetDirection);
            managedTransform.rotation = Quaternion.Slerp(managedTransform.rotation, finalRotation, Time.deltaTime * ship.AngularVelocity * TurnDampening);
        }

        /**
         * At close range, the target will be too close to the attacker and will be "overshot", since the guns' focal point it too far.
         * This method compensates for it by aiming _one_ particular gun at the target.
         * While working well enough, the method sometimes interfers with the path finding, which creates variation.
         */
        private void AdjustForCloseRange()
        {
            Debug.Log("AdjustForCloseRange()");

            // Determine closest gun
            Vector3 relativePoint = managedTransform.InverseTransformPoint(target.transform.position);
            Vector3 gunOffset = relativePoint.x < 0.0 ? LeftGunOffset : RightGunOffset;

            Vector3 center2Target = managedShip.transform.position - target.transform.position;
            Vector3 gun2FocalPoint = managedTransform.forward.normalized * Beam.MaxRange - (managedShip.transform.position + gunOffset);
            Vector3 hotspot = (managedShip.transform.position + gunOffset) + (gun2FocalPoint.normalized * (center2Target.magnitude + Mathf.Abs(gunOffset.z))); // Compensate for possible gun displacement
            Vector3 target2Hotspot = target.transform.position - hotspot;

            if (Debug.isDebugBuild) // Save some vector calculations...
            {
                // Center of managed ship to target
                Debug.DrawLine(managedShip.transform.position, target.transform.position, Color.blue, 0.05f);
                // Gun to gun focal point (250m ahead)
                Debug.DrawLine(managedShip.transform.position + gunOffset, managedTransform.forward.normalized * Beam.MaxRange, Color.red, 0.05f);
                // Same direction as the line to the gun focal point, but as short as the distance from the ship to the target
                Debug.DrawLine(managedShip.transform.position + gunOffset, hotspot, Color.yellow, 0.05f);
                Debug.DrawLine(target.transform.position, hotspot, Color.magenta, 0.05f);
            }

            if (target2Hotspot.magnitude > 50)
            {
                // Use normal turning
                TurnTowards(target.transform.position);
            }
            else if (target2Hotspot.magnitude < 50 && target2Hotspot.magnitude > 5)
            {
                // Yes, 0.5f is so truly a magic constant that it didn't get a name
                managedTransform.Rotate(0, target2Hotspot.x > 0 ? 1.5f : -1.5f, 0);
                managedTransform.Rotate(target2Hotspot.y > 0 ? -1.5f : 1.5f, 0, 0);
            }
            else
            {
                managedTransform.Rotate(0, target2Hotspot.x > 0 ? 0.25f : -0.25f, 0);
                managedTransform.Rotate(target2Hotspot.y > 0 ? -0.25f : 0.25f, 0, 0);
            }

            closeRangeAdjustmentPerformed = target2Hotspot.magnitude <= FiringDistanceTreshhold;
        }
    }
}
