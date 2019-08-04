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

        private bool isAlive = true;

        private int maxHullPoints;
        private int maxShieldPoints;
        private int hullPoints;
        private int shieldPoints;

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
            get => (int)(100 * (float)hullPoints / maxHullPoints);
        }

        public int ShieldPoints
        {
            get => maxShieldPoints == 0 ? 0 : (int)(100 * (float)shieldPoints / maxShieldPoints);
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

        void Awake()
        {
            inspectable = GetComponent<Inspectable>();
        }

        void Start()
        {
            hullPoints = maxHullPoints = shipData.HullPoints;
            shieldPoints = maxShieldPoints = shipData.ShieldPoints;

            EventManager.RaiseNewShipEntered(this);
        }

        public void OnTriggerEnter(Collider other)
        {
            if (GameObjects.IsBeam(other) && isAlive)
            {
                Beam beam = other.GetComponentInParent<Beam>();

                // Don't self kill. Sometimes the beam is spawned too close to the emitting craft and would damage it.
                if (beam.owner == GameObjects.GetParentShip(transform.gameObject))
                {
                    return;
                }

                other.gameObject.SetActive(false);
                Destroy(other.gameObject);
                if (shieldPoints > 0)
                {
                    BroadcastMessage("OnShieldImpact");
                    shieldPoints = Mathf.Max(shieldPoints - 10, 0);
                }
                else
                {
                    hullPoints = Mathf.Max(hullPoints - 10, 0);
                }

                EventManager.RaiseShipHit(gameObject.name);
                if (hullPoints <= 0)
                {
                    if (GameObjects.IsPlayer(name))
                    {
                        GetComponent<PlayerCollisionHandler>().Die("You were shot down");
                    }
                    else
                    {
                        isAlive = false;
                        gameObject.BroadcastMessage("Explode");
                        EventManager.RaiseShipDestroyed(gameObject.name);
                        Destroy(gameObject, 0.1f);
                    }
                }
            }
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

        public void Turn(float intensity) {
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
            speed = Mathf.Clamp(speed + Acceleration * deltaTime, 0, MaxSpeed);
        }

        public void DeccelerateByDelta(float deltaTime)
        {
            speed = Mathf.Clamp(speed - Acceleration * deltaTime, 0, MaxSpeed);
        }

        #endregion
    }
}