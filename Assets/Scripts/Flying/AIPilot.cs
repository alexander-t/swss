using AI;
using Core;
using System;
using UnityEngine;

namespace Flying
{
    [RequireComponent(typeof(Ship))]
    public class AIPilot : MonoBehaviour
    {
        public Transform[] waypoints;
        public Transform rightGunTransform;
        public Transform leftGunTransform;

        private Ship ship;
        private Behavior behavior;

        private PathFinding pathFinding;

        void Awake()
        {
            ship = GetComponent<Ship>();
        }

        void Start()
        {
            pathFinding = new PathFinding(transform);
            behavior = new PatrollingBehavior(gameObject, waypoints);
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
            catch (BehaviorNotApplicableException)
            {
                behavior = new PatrollingBehavior(gameObject, waypoints);
                behavior.Commence();
            }
            Debug.Log(behavior.Describe());
        }

        public void Attack(GameObject target) {
            behavior = new AttackingBehavior(gameObject, rightGunTransform, leftGunTransform, target);
            behavior.Commence();
        }

        private void Move()
        {
            transform.position += transform.forward * Time.deltaTime * ship.Velocity;
        }

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

    }
}