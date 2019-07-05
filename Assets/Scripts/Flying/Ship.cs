using Core;
using Targeting;
using UnityEngine;

namespace Flying
{
    public class Ship : MonoBehaviour, Targettable
    {
        public ShipData shipData;

        // Mission and targeting computer aren't required, because communications with them happens via messages
        private GameObject mission;
        private GameObject targetingComputer;
        private bool isDestroyed = false;

        private int maxHullPoints;
        private int maxShieldPoints;
        private int hullPoints;
        private int shieldPoints;

        // Movement
        private const float Acceleration = 20;
        private Vector3 direction = Vector3.zero;
        private float velocity = 20;

        #region Properties
        public string Name
        {
            get => name;
        }

        public ShipFaction ShipFaction
        {
            get => shipData.ShipFaction;
        }

        public int HullPoints
        {
            get => hullPoints;
        }

        public int ShieldPoints
        {
            get => shieldPoints;
        }

        public float MaxSpeed
        {
            // Recalculate here to go from actual kmph to game speed
            get => shipData.MaxSpeed / 10;
        }

        public float AngularVelocity
        {
            get => shipData.AngularVelocity;
        }

        public float Velocity
        {
            get => velocity;
        }

        public Vector3 Direction
        {
            get => direction;
        }

        #endregion
        void Awake()
        {
            mission = GameObject.Find(Constants.Mission);
            targetingComputer = GameObject.Find(Constants.TargetingComputer);
        }

        void Start() {
            hullPoints = maxHullPoints = shipData.HullPoints;
            shieldPoints = maxShieldPoints = shipData.ShieldPoints;
            if (targetingComputer != null)
            {
                // Targeting computer may be null in test scenes
                targetingComputer.BroadcastMessage("OnNewTarget", this);
            }
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(Constants.Tag_LaserBeam) && !isDestroyed)
            {
                Beam beam = other.GetComponentInParent<Beam>();
                
                // Don't self kill. Sometimes the beam is spawned too close to the emitting craft and would damage it.
                if (beam.owner == GameObjects.GetParentShip(transform.gameObject)) {
                    return;
                }

                Destroy(other.gameObject);

                if (shieldPoints > 0)
                {
                    BroadcastMessage("OnShieldImpact");
                    shieldPoints -= 10;
                }
                else
                {
                    hullPoints -= 10;
                }

                targetingComputer.BroadcastMessage("OnEnemyHit");
                if (hullPoints <= 0)
                {
                    isDestroyed = true;

                    gameObject.BroadcastMessage("Explode");

                    mission.BroadcastMessage("OnEnemyDestroyed", gameObject.name);
                    targetingComputer.BroadcastMessage("OnEnemyDestroyed", gameObject.name);

                    Destroy(gameObject, 0.1f);

                }
            }
        }

        #region Movement control
        public void BeginManeuver()
        {
            direction = Vector3.zero;
        }

        public void PitchDown()
        {
            direction.x = AngularVelocity;
        }

        public void PitchUp()
        {
            direction.x -= AngularVelocity;
        }

        public void TurnLeft()
        {
            direction.y -= AngularVelocity;
            direction.z += AngularVelocity;
        }

        public void TurnRight()
        {
            direction.y += AngularVelocity;
            direction.z -= AngularVelocity;
        }

        public void RollRight()
        {
            direction.z -= AngularVelocity;
        }

        public void RollLeft()
        {
            direction.z += AngularVelocity;
        }

        public void Halt()
        {
            velocity = 0;
        }

        public void AccelerateByDelta(float deltaTime)
        {
            velocity = Mathf.Clamp(velocity + Acceleration * deltaTime, 0, MaxSpeed);
        }

        public void DeccelerateByDelta(float deltaTime)
        {
            velocity = Mathf.Clamp(velocity - Acceleration * deltaTime, 0, MaxSpeed);
        }

        #endregion
    }
}