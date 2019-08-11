using AI;
using Core;
using System;
using System.Collections;
using System.Collections.Generic;
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

        private Ship ship;
        private AITargetingComputer targetingComputer;
        private GameObject player;
        private List<GameObject> enemies = new List<GameObject>();

        private PathFinding pathFinding;
        private Behavior behavior;

        void Awake()
        {
            ship = GetComponent<Ship>();
            targetingComputer = GetComponent<AITargetingComputer>();

            player = GameObject.Find(Constants.Player);
        }

        void Start()
        {
            pathFinding = new PathFinding(transform);
            behavior = DetermineInitialBehavior();
            behavior.Commence();
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
                    behavior = DetermineInitialBehavior();
                    behavior.Commence();
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
                             
        private Behavior DetermineInitialBehavior()
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
       
        #region Triggers
        void OnTriggerEnter(Collider other)
        {
            try
            {
                // Behavior may be null because of a race condition during initialization and the trigger :(
                if (behavior != null)
                {
                    behavior.TargetEnteredKillzone(GameObjects.GetParentShip(other.gameObject));
                }
            }
            catch (ArgumentException)
            {
                // Quite ok. Not everything is a ship
            }
        }

        void OnTriggerExit(Collider other)
        {
            try
            {
                if (behavior != null)
                {
                    behavior.TargetLeftKillzone(GameObjects.GetParentShip(other.gameObject));
                }
            }
            catch (ArgumentException)
            {
                // Quite ok. Not everything is a ship
            }
        }
        #endregion
    }
}