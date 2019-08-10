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
        private GameObject player;
        private List<GameObject> enemies = new List<GameObject>();

        private PathFinding pathFinding;
        private Behavior behavior;

        void Awake()
        {
            ship = GetComponent<Ship>();
            player = GameObject.Find(Constants.Player);
        }

        void Start()
        {
            StartCoroutine(UpdateEnemyDistanceMap());
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
            catch (BehaviorNotApplicableException)
            {
                behavior = DetermineInitialBehavior();
                behavior.Commence();
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

        private ShipFaction GetEnemyFaction()
        {
            if (ship.ShipFaction == ShipFaction.Neutral)
            {
                throw new InvalidOperationException("Can't determine enemy faction for neutral ship " + ship.name);
            }
            return ship.ShipFaction == ShipFaction.Alliance ? ShipFaction.Empire : ShipFaction.Alliance;
        }

        private List<GameObject> GetEnemies()
        {
            List<GameObject> enemies = new List<GameObject>();
            ShipFaction enemyFaction = GetEnemyFaction();

            foreach (GameObject go in GameObject.FindGameObjectsWithTag(Constants.Tag_Ship))
            {
                if (go.GetComponent<Ship>().ShipFaction == enemyFaction)
                {
                    enemies.Add(go);
                }
            }
            return enemies;
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
                    return new ScanningBehavior(gameObject);
                }
                else
                {
                    return new PatrollingBehavior(gameObject, waypoints);
                }
            }
        }

        IEnumerator UpdateEnemyDistanceMap()
        {
            while (true)
            {
                string s = "";
                foreach (GameObject e in GetEnemies()) {
                    s += e.name + "(" + Vector3.Distance(transform.position, e.transform.position) + ") ";
                }
                Debug.Log(name + " > " + s);
                yield return new WaitForSeconds(0.2f);
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