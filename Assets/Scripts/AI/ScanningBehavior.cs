using Flying;
using UnityEngine;

namespace AI
{
    public class ScanningBehavior : Behavior
    {
        private Vector3 destination;

        private Transform managedTransform;
        private Ship ship;

        public ScanningBehavior(GameObject managedShip)
        {
            managedTransform = managedShip.transform;
            ship = managedShip.GetComponent<Ship>();
        }

        public void Commence()
        {
            ship.Speed = 50;
            destination = managedTransform.forward * 1000;
        }

        public void Turn()
        {
            if (false)
            {
                throw new BehaviorNotApplicableException(BehaviorChangeReason.TargetAcquired);
            }

            if (Vector3.Distance(managedTransform.position, destination) < 25) {
                destination = managedTransform.forward * -1000;
            }
            TurnTowards(destination);
        }

        public string Describe()
        {
            return ship.name + " scanning; going to " + destination;
        }

        public void Attack()
        {
            // Do nothing   
        }

        public void TargetEnteredKillzone(GameObject potentialTarget)
        {
            // Do nothing
        }

        public void TargetLeftKillzone(GameObject potentialTarget)
        {
            // Do nothing
        }

        private void TurnTowards(Vector3 target)
        {
            const float TurnDampening = 0.01f;
            Vector3 targetDirection = target - managedTransform.position;
            Quaternion finalRotation = Quaternion.LookRotation(targetDirection);
            managedTransform.rotation = Quaternion.Slerp(managedTransform.rotation, finalRotation, Time.deltaTime * ship.AngularVelocity * TurnDampening);
        }

    }
}
