using Firing;
using Flying;
using UnityEngine;

namespace AI
{
    public enum Action
    {
        Attacking, Disengaging
    }

    public class AttackingBehavior : Behavior
    {
        private const float TurnDampening = 0.03f;

        private const float LongRange = 250;
        private const float LongRangeAttackVelocity = 75;

        private const float MediumRange = 175;
        private const float MediumRangeAttackVelocity = 50;

        private const float CloseRange = 100;
        private const float CloseRangeAttackVelocity = 40;

        private Transform managedTransform;
        private GameObject managedShip;
        private Transform rightGunTransform;
        private Transform leftGunTransform;
        private Ship ship;
        private bool targetInKillZone;

        private GameObject target;

        // Used to determine whether to show down and turn or disengage and reingage.
        private const float RemainingTurnAngle = 20;

        // Disengage if within this distance
        private const float MinimumAttackDistance = 75;

        // Distances used for disengagement
        private const float MinimumDisengagementDistance = 125;
        private const float MaximumDisengagementDistance = 250;

        // Probability that an attacker will disengage in the midst of an ongoing attack. Adds some randomness.
        private const float ProbabilityToDisengageDuringAttack = 0.1f;

        // Proximity to the "waypoint" when disengaging. Should be big enough to avoid circeling behavior.
        private const float DistanceToDisengagementWaypoint = 25;

        private Action action = Action.Attacking;
        private Vector3 reengagementPosition = Vector3.zero;
        private float nextReengagementTime;

        private float nextStrategyReevaluationTime;

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
            ship.Speed = DetermineAttackSpeed(target.transform);
        }

        public void Turn()
        {
            if (target == null)
            {
                throw new BehaviorNotApplicableException(BehaviorChangeReason.TargetDestroyed);
            }

            Vector3 targetPosition = target.transform.position;

            if (action == Action.Attacking)
            {
                reengagementPosition = Vector3.zero;

                ship.Speed = DetermineAttackSpeed(target.transform);
                TurnTowards(target.transform.position);
                if (Vector3.Distance(managedTransform.position, targetPosition) < MinimumAttackDistance
                    && MustTurnTooFarToFaceTarget(targetPosition))
                {
                    action = Action.Disengaging;
                }

                if (Time.time >= nextStrategyReevaluationTime)
                {
                    if (Random.value > 1 - ProbabilityToDisengageDuringAttack)
                    {
                        action = Action.Disengaging;
                    }
                    nextStrategyReevaluationTime = Time.time + Random.Range(5, 19);
                }

                if (Debug.isDebugBuild)
                {
                    Debug.DrawLine(managedTransform.position, targetPosition, Color.white, 0.01f);
                    Vector3 preferredAttackPosition = (managedTransform.position - targetPosition).normalized * MinimumAttackDistance + target.transform.position;
                    Debug.DrawLine(managedTransform.position, preferredAttackPosition, Color.red, 0.01f);
                }
            }
            else if (action == Action.Disengaging)
            {
                // Get away asap
                ship.Speed = LongRangeAttackVelocity; 
                if (reengagementPosition == Vector3.zero)
                {
                    // There's both a reengagement position and a time, whichever happens first wins.
                    reengagementPosition = managedTransform.forward * Random.Range(MinimumDisengagementDistance, MaximumDisengagementDistance);
                    nextReengagementTime = Time.time + Random.Range(10, 20);
                }
                else
                {
                    TurnTowards(reengagementPosition);
                    if (Vector3.Distance(managedTransform.position, reengagementPosition) <= DistanceToDisengagementWaypoint)
                    {
                        action = Action.Attacking;
                    }

                    if (Time.time >= nextReengagementTime)
                    {
                        action = Action.Attacking;
                    }
                }

                Debug.DrawLine(managedTransform.position, reengagementPosition, Color.blue, 0.01f);
            }

        }

        public void Attack()
        {
            if (target != null 
                && IsTargetAlmostDeadAhead(target.transform.position) 
                && Beam.IsTargetInKillRange(managedTransform.position, target.transform.position))
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

        private bool MustTurnTooFarToFaceTarget(Vector3 targetPosition)
        {
            Vector3 targetDirection = targetPosition - managedTransform.position;
            return Vector3.Angle(targetDirection, managedTransform.forward) > RemainingTurnAngle;
        }

        private bool IsTargetAlmostDeadAhead(Vector3 targetPosition)
        {
            float distanceToTarget = Vector3.Distance(managedTransform.position, targetPosition);
            if (distanceToTarget > Beam.MaxRange / 2)
            {
                return Vector3.Distance((targetPosition - managedTransform.position).normalized, managedTransform.forward) <= 0.25;
            }
            else {
                return Vector3.Distance((targetPosition - managedTransform.position).normalized, managedTransform.forward) <= 0.5;
            }
        }
               
        private float DetermineAttackSpeed(Transform targetTransform)
        {
            float distanceToTarget = Vector3.Distance(managedTransform.position, targetTransform.position);
            if (distanceToTarget <= CloseRange)
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


/*
private GameObject GetRandomEnemy()
{
    if (enemies.Length > 0)
    {
        return enemies[Random.Range(0, enemies.Length)];
    }
    return null;
}

private GameObject GetClosestEnemy()
{
    Vector3 position = transform.position;
    float minDistance = float.MaxValue;
    GameObject closetEnemy = null;

    foreach (GameObject enemy in enemies)
    {
        float distance = (enemy.transform.position - position).sqrMagnitude;
        if (distance < minDistance)
        {
            minDistance = distance;
            closetEnemy = enemy;
        }
    }
    return closetEnemy;
}
*/

