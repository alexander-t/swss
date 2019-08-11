using Flying;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    public class ScanningBehavior : Behavior
    {
        private Vector3 destination;

        private Transform managedTransform;
        private AITargetingComputer targetingComputer;
        private Ship ship;

        public ScanningBehavior(GameObject managedShip, AITargetingComputer targetingComputer)
        {
            managedTransform = managedShip.transform;
            this.targetingComputer = targetingComputer;
            ship = managedShip.GetComponent<Ship>();
        }

        public void Commence()
        {
            ship.Speed = 50;
            destination = managedTransform.forward * 1500;
        }

        public void Turn()
        {
            // TODO: Unparemeterized magic number to be fixed later
            List<GameObject> potentialTargets = targetingComputer.GetTargetsWithinDistance(3000); 
            if (potentialTargets.Count > 0)
            {
                throw new BehaviorNotApplicableException(BehaviorChangeReason.TargetAcquired, potentialTargets[Random.Range(0, potentialTargets.Count)]);
            }

            if (Vector3.Distance(managedTransform.position, destination) < 25) {
                destination = managedTransform.forward * -1500;
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
