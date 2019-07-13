using Flying;
using UnityEngine;

namespace AI
{
    public class AttackingBehavior : Behavior
    {
        private const float TurnDampening = 0.03f;
        private const float FiringDistanceTreshhold = 5.0f;

        private const float LongRange = 250;
        private const float LongRangeAttackVelocity = 50;

        private const float MediumRange = 175;
        private const float MediumRangeAttackVelocity = 25;

        private const float CloseRange = 100;
        private const float CloseRangeAttackVelocity = 10;

        private const float DeadAheadRange = 50;
        private const float DeadAheadAttackVelocity = 5;


        private Transform managedTransform;
        private GameObject managedShip;
        private Transform rightGunTransform;
        private Transform leftGunTransform;
        private Ship ship;
        private bool targetInKillZone;
        private bool closeRangeAdjustmentPerformed;

        private GameObject target;

        public AttackingBehavior(GameObject managedShip, Transform rightGunTransform, Transform leftGunTransform, GameObject target)
        {
            this.managedShip = managedShip;
            this.rightGunTransform = rightGunTransform;
            this.leftGunTransform = leftGunTransform;
            this.target = target;
            managedTransform = managedShip.transform;
            ship = managedShip.GetComponent<Ship>();
        }


        public void Commence()
        {
            ship.Velocity = DetermineAttackVelocity(target.transform);
        }

        public void Turn()
        {
            if (target == null)
            {
                throw new BehaviorNotApplicableException();
            }

            ship.Velocity = DetermineAttackVelocity(target.transform);

            if (Vector3.Distance(target.transform.position, managedTransform.position) < CloseRange)
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
            Vector3 gunPosition = DetermineClosestGun(target.transform.position);
            float gunXOffset = Mathf.Abs(gunPosition.x - managedTransform.position.x);
            Vector3 center2Target = managedTransform.position - target.transform.position;
            Vector3 gun2FocalPoint = (managedTransform.position + managedTransform.forward * Beam.MaxRange) - gunPosition;
            Vector3 hotspot = gunPosition + (gun2FocalPoint.normalized * (center2Target.magnitude + gunXOffset));
            Vector3 target2Hotspot = target.transform.position - hotspot;

            if (Debug.isDebugBuild) // Save some vector calculations...
            {
                // Center of managed ship to target
                Debug.DrawLine(managedTransform.position, target.transform.position, Color.blue);
                // Gun to gun focal point (250m ahead), also drawn by LaserBeamEmitter
                // Debug.DrawLine(managedTransform.position + gunOffset, managedTransform.position + managedTransform.forward.normalized * Beam.MaxRange, Color.red);
                // Same direction as the line to the gun focal point, but as short as the distance from the ship to the target
                Debug.DrawLine(gunPosition, hotspot, Color.yellow);
                Debug.DrawLine(target.transform.position, hotspot, Color.magenta);
            }

            if (target2Hotspot.magnitude > 25)
            {
                // Use normal turning
                TurnTowards(target.transform.position);
            }
            else if (target2Hotspot.magnitude <= 25 && target2Hotspot.magnitude > 10)
            {
                Debug.Log("x=" + target2Hotspot.x + ", y=" + target2Hotspot.y + ", " + target2Hotspot.magnitude);
                float t = 0.5f;
                managedTransform.Rotate(0, target2Hotspot.x > 0 ? t : -t, 0);
                managedTransform.Rotate(target2Hotspot.y > 0 ? -t : t, 0, 0);
            }
            else
            {
                Debug.Log("x=" + target2Hotspot.x + ", y=" + target2Hotspot.y + ", " + target2Hotspot.magnitude);
                float t = 0.25f;
                managedTransform.Rotate(0, target2Hotspot.x > 0 ? t : -t, 0);
                managedTransform.Rotate(target2Hotspot.y > 0 ? -t : t, 0, 0);
            }

            closeRangeAdjustmentPerformed = target2Hotspot.magnitude <= FiringDistanceTreshhold;
        }

        private Vector3 DetermineClosestGun(Vector3 targetPosition)
        {
            float distanceToLeftGun = Vector3.Distance(leftGunTransform.position, targetPosition);
            float distanceToRightGun = Vector3.Distance(rightGunTransform.position, targetPosition);
            return distanceToLeftGun <= distanceToRightGun ? leftGunTransform.position : rightGunTransform.position;
        }

        private float DetermineAttackVelocity(Transform targetTransform)
        {
            float distanceToTarget = Vector3.Distance(managedTransform.position, targetTransform.position);
            if (distanceToTarget <= DeadAheadRange)
            {
                return DeadAheadAttackVelocity;
            }
            if (distanceToTarget > DeadAheadRange && distanceToTarget <= CloseRange)
            {
                return CloseRangeAttackVelocity;
            }
            else if (distanceToTarget > CloseRange && distanceToTarget <= MediumRange)
            {
                return MediumRangeAttackVelocity;
            }
            else
            {
                return LongRangeAttackVelocity;
            }
        }
    }
}
