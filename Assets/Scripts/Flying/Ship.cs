using Targeting;
using UnityEngine;

namespace Flying
{
    public class Ship : MonoBehaviour, Targettable
    {
        public ShipData shipData;

        // Bookkeeping 
        private GameObject mission;
        private GameObject targetingComputer;
        private bool isDestroyed = false;

        // Targettable
        private int hullPoints;
        private int shieldPoints;

        // Movement
        private const float Acceleration = 20;
        private Vector3 direction = Vector3.zero;
        private float velocity = 20;
        private float maxVelocity;
        private float angularVelocity;

        #region Properties
        public string Name
        {
            get => name;
        }

        public int HullPoints
        {
            get => hullPoints;
        }

        public int ShieldPoints
        {
            get => shieldPoints;
        }

        public float MaxVelocity
        {
            get => maxVelocity;
        }

        public float Velocity
        {
            get => velocity;
        }

        public ShipFraction ShipFraction
        {
            get => shipData.ShipFraction;
        }

        public Vector3 Direction
        {
            get => direction;
        }

        #endregion
        void Awake()
        {
            mission = GameObject.Find("Mission");
            targetingComputer = GameObject.Find("Targeting Computer");

            hullPoints = shipData.HullPoints;
            shieldPoints = shipData.ShieldPoints;
            maxVelocity = shipData.MaxVelocity;
            angularVelocity = shipData.AngularVelocity;
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(Constants.LaserBeam) && !isDestroyed)
            {
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
            direction.x = angularVelocity;
        }

        public void PitchUp()
        {
            direction.x -= angularVelocity;
        }

        public void TurnLeft()
        {
            direction.y -= angularVelocity;
            direction.z += angularVelocity;
        }

        public void TurnRight()
        {
            direction.y += angularVelocity;
            direction.z -= angularVelocity;
        }

        public void RollRight()
        {
            direction.z -= angularVelocity;
        }

        public void RollLeft()
        {
            direction.z += angularVelocity;
        }

        public void Halt()
        {
            velocity = 0;
        }

        public void AccelerateByDelta(float deltaTime)
        {
            velocity = Mathf.Clamp(velocity + Acceleration * deltaTime, 0, MaxVelocity);
        }

        public void DeccelerateByDelta(float deltaTime)
        {
            velocity = Mathf.Clamp(velocity - Acceleration * deltaTime, 0, MaxVelocity);
        }

        #endregion
    }
}