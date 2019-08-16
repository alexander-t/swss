using AI;
using Core;
using UnityEngine;

namespace Flying
{
    [RequireComponent(typeof(Ship))]
    public class AIPilot : MonoBehaviour
    {
        public Transform[] waypoints;
        public Transform rightGunTransform;
        public Transform leftGunTransform;
        public BehaviorName defaultBehavior = BehaviorName.Patrolling;
        public Personality personality = Personality.Neutral;
        public RevengeTenacity revengeTenacity = RevengeTenacity.Flaky;

        private Ship ship;
        private AITargetingComputer targetingComputer;
        private GameObject player;
        private GameObject enemyToRetaliateAgainst; // An attacker who's fired a beam at this AI pilot

        private PathFinding pathFinding;
        private Behavior behavior;

        void Awake()
        {
            ship = GetComponent<Ship>();
            targetingComputer = GetComponent<AITargetingComputer>();

            player = GameObject.Find(Constants.Player);

            EventManager.onShipHit += OnShipHit;
        }

        void Start()
        {
            pathFinding = new PathFinding(transform);
            behavior = DetermineDefaultBehavior();
            behavior.Commence();

        }

        void OnDestroy()
        {
            EventManager.onShipHit -= OnShipHit;
        }

        void Update()
        {
            try
            {
                Direction availableDirections = pathFinding.FirstPassRaycast();
                if (availableDirections == Direction.None)
                {
                    Debug.Log("STUCK! Cheating!");
                    transform.Rotate(0, 180, 0);
                }
                else
                {
                    // Do collision detection; if nothing is blocking, let the behavior decide the movement
                    Vector3 turningDirection = pathFinding.DetermineTurningDirection(availableDirections);
                    if (turningDirection != Vector3.zero)
                    {
                        transform.Rotate(turningDirection * ship.AngularVelocity * Time.deltaTime);
                    }
                    else
                    {
                        behavior.Turn();
                    }
                    Move();
                }
                behavior.Attack();
            }
            catch (BehaviorNotApplicableException e)
            {
                if (e.Reason == BehaviorChangeReason.TargetAcquired)
                {
                    Attack(e.Target);
                }
                else
                {
                    behavior = DetermineDefaultBehavior();
                    behavior.Commence();
                    
                    // Forget whoever fired at this ship
                    enemyToRetaliateAgainst = null;
                }
            }
            Debug.Log(behavior.Describe());
        }

        public void Attack(GameObject target)
        {
            behavior = new AttackingBehavior(gameObject, rightGunTransform, leftGunTransform, target);
            behavior.Commence();
        }

        private void Move()
        {
            transform.position += transform.forward * Time.deltaTime * ship.Speed;
        }
                             
        private Behavior DetermineDefaultBehavior()
        {
            if (personality == Personality.Aggressive)
            {
                // Don't care about default behavior if the AI is aggressive
                return new AttackingBehavior(gameObject, rightGunTransform, leftGunTransform, player);
            }
            else
            {
                if (defaultBehavior == BehaviorName.Scanning)
                {
                    return new ScanningBehavior(gameObject, targetingComputer);
                }
                else
                {
                    return new PatrollingBehavior(gameObject, waypoints);
                }
            }
        }

        private void OnShipHit(string attacked, string attackerName)
        {
            if (name == attacked)
            {
                GameObject attacker = targetingComputer.GetTargetByName(attackerName);
                if (attacker != null) {
                    if (enemyToRetaliateAgainst == null)
                    {
                        enemyToRetaliateAgainst = attacker;
                        Attack(enemyToRetaliateAgainst);
                    }
                    else
                    {
                        if (attacker != enemyToRetaliateAgainst && revengeTenacity == RevengeTenacity.Flaky) {
                            enemyToRetaliateAgainst = attacker;
                            Attack(enemyToRetaliateAgainst);
                        }
                    }
                }
            }
        }
    }
}