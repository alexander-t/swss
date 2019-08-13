using Core;
using Firing;
using Targeting;
using UnityEngine;

namespace Flying
{
    public class Ship : MonoBehaviour, Targettable
    {
        public ShipData shipData;

        private Inspectable inspectable;
        private PowerDistributionSystem powerDistributionSystem;

        private bool isAlive = true;

        private int maxHullPoints;
        private int maxShieldPoints;
        private float hullPoints;
        private float shieldPoints;

        // Movement
        private const float Acceleration = 20;
        private Vector3 direction = Vector3.zero;
        private float speed = 20;

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
            get => (int)(100 * hullPoints / maxHullPoints);
        }

        public int ShieldPoints
        {
            get => maxShieldPoints == 0 ? 0 : (int)(100f * shieldPoints / maxShieldPoints);
        }

        public string Contents
        {
            get => inspectable != null ? inspectable.VisibleContents : "";
        }

        public float MaxSpeed
        {
            // Recalculate here to go from actual kmph to game speed
            get => shipData.MaxSpeed / 10;
        }

        public float MaxSpeedAdjustedForPowerdistribution {
            get => powerDistributionSystem == null ? MaxSpeed : MaxSpeed * powerDistributionSystem.EnginePower;
        }

        public float AngularVelocity
        {
            get => shipData.AngularVelocity;
        }

        public float Speed
        {
            get => speed;
            set => speed = value; // TODO: temporary to test AI
        }

        public Vector3 Direction
        {
            get => direction;
        }

        #endregion

        #region Unity lifecycle
        void Awake()
        {
            inspectable = GetComponent<Inspectable>();
            powerDistributionSystem = GetComponentInParent<PowerDistributionSystem>();
        }

        void Start()
        {
            hullPoints = maxHullPoints = shipData.HullPoints;
            shieldPoints = maxShieldPoints = shipData.ShieldPoints;

            EventManager.RaiseNewShipEntered(this);
        }

        void Update() {
            // A change in the power distribution may make this happen, so adjust immediately.
            if (Speed > MaxSpeedAdjustedForPowerdistribution) {
                Speed = MaxSpeedAdjustedForPowerdistribution;
                EventManager.RaisePlayerSpeedChanged(Speed, MaxSpeed);
            }
        }

        public void OnTriggerEnter(Collider other)
        {
            if (!isAlive)
            {
                return;
            }

            if (GameObjects.IsThreatArea(other) || GameObjects.IsShieldSphere(other))
            {
                // Ignore firing components' threat areas and shield spheres.
                return;
            }
            else if (GameObjects.IsBeam(other))
            {
                Beam beam = other.GetComponentInParent<Beam>();

                // Don't self kill. Sometimes the beam is spawned too close to the emitting craft and would damage it.
                if (beam.Owner == GameObjects.GetParentShip(transform.gameObject))
                {
                    return;
                }

                string beamOwner = beam.Owner.name;
                float beamDamage = Beam.BaseDamage * beam.Intensity;
                beam.Destroy();

                if (shieldPoints > 0)
                {
                    BroadcastMessage("OnShieldImpact");
                    shieldPoints = Mathf.Max(shieldPoints - beamDamage , 0);
                }
                else
                {
                    hullPoints = Mathf.Max(hullPoints - beamDamage, 0);
                }

                EventManager.RaiseShipHit(gameObject.name);
                if (hullPoints <= 0)
                {
                    isAlive = false;
                    if (GameObjects.IsPlayer(this))
                    {
                        KillPlayer("You were shot down by " + beamOwner);
                    }
                    else
                    {
                        EventManager.RaiseShipDestroyed(gameObject.name);
                        Destroy(gameObject, 0.1f);
                    }
                }
            }
            else if (GameObjects.IsAsteroid(other))
            {
                if (GameObjects.IsPlayer(this))
                {
                    isAlive = false;
                    KillPlayer("You crashed into an asteroid");
                }
            }
            else 
            {
                if (IsSelf(other))
                {
                    // Ignore collisions that occur between the ship's kinematic collider and its triggers
                    return;
                }

                if (GameObjects.IsPlayer(this))
                {
                    isAlive = false;
                    KillPlayer("You crashed");
                }
            }
        }
        #endregion

        private bool IsSelf(Collider other)
        {
            return this == GameObjects.GetParentShip(other.gameObject)?.GetComponent<Ship>();
        }

        private void KillPlayer(string reason) {
            GetComponentInParent<PlayerDeath>().Die(reason);
        }

        #region Movement control
        public void BeginManeuver()
        {
            direction = Vector3.zero;
        }

        public void Pitch(float intensity)
        {
            direction.x = intensity * AngularVelocity;
        }

        public void Turn(float intensity)
        {
            direction.y = intensity * AngularVelocity;
            direction.z -= intensity * AngularVelocity;
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
            speed = 0;
        }

        public void AccelerateByDelta(float deltaTime)
        {
            speed = Mathf.Clamp(speed + Acceleration * deltaTime, 0, MaxSpeedAdjustedForPowerdistribution);
        }

        public void DeccelerateByDelta(float deltaTime)
        {
            speed = Mathf.Clamp(speed - Acceleration * deltaTime, 0, MaxSpeedAdjustedForPowerdistribution);
        }

        #endregion
    }
}