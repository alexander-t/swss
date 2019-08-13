using Core;
using Flying;
using UnityEngine;

namespace Firing
{
    /*
     * Emits laser beams originating from two different lasers with some cooldown. 
     */
    public class LaserBeamEmitter : MonoBehaviour
    {
        private enum FiringMode { Single, Dual };

#pragma warning disable 0649
        [SerializeField]
        private GameObject laserBeamPrefab;
        [SerializeField]
        private GameObject rightGun;
        [SerializeField]
        private GameObject leftGun;
        [Space(7)]
        [SerializeField]
        private AudioSource audioSource;
        [SerializeField]
        private AudioClip laserSound;
#pragma warning restore 0649

        private const float Cooldown = 0.2f;

        private Ship ship;
        private BeamPool beamPool;
        private PowerDistributionSystem powerDistributionSystem;
        private bool rightGunFiringNext = true;
        private FiringMode firingMode = FiringMode.Single;
        private float nextShotTime;

        void Awake()
        {
            ship = GetComponent<Ship>();
            powerDistributionSystem = GetComponentInParent<PowerDistributionSystem>();
            beamPool = GameObject.Find(Constants.BeamPool).GetComponent<BeamPool>();
            audioSource.clip = laserSound;
        }

        void Update()
        {
            if (GameObjects.IsPlayer(transform.root.name))
            {
                // Control only player's beams with keyboard. Do the rest via events.
                if (Input.GetKeyUp(KeyCode.X))
                {
                    ToggleFiringMode();
                }
                if (Input.GetAxis("Fire1") != 0)
                {
                    Fire();
                }
            }

            if (Debug.isDebugBuild)
            {
                Debug.DrawLine(transform.position + transform.forward.normalized * Beam.MaxRange, rightGun.transform.position, Color.red, 0.1f);
                Debug.DrawLine(transform.position + transform.forward.normalized * Beam.MaxRange, leftGun.transform.position, Color.red, 0.1f);
            }
        }

        void OnFire()
        {
            Fire();
        }

        private void Fire()
        {
            if (Time.time > nextShotTime)
            {
                if (firingMode == FiringMode.Single)
                {
                    FireGun(rightGunFiringNext ? rightGun : leftGun);
                    rightGunFiringNext = !rightGunFiringNext;
                    nextShotTime = Time.time + Cooldown;
                }
                else if (firingMode == FiringMode.Dual)
                {
                    FireGun(rightGun);
                    FireGun(leftGun);
                    nextShotTime = Time.time + Cooldown * 2;
                }
                audioSource.Play();
            }
        }

        private void FireGun(GameObject gun)
        {
            Vector3 gunFocalPoint = transform.position + transform.forward * Beam.MaxRange;
            LaserColor beamColor = (ship.ShipFaction == ShipFaction.Empire) ? LaserColor.Green : LaserColor.Orange;
            float initalIntensity = powerDistributionSystem == null ? 1 : powerDistributionSystem.LaserPower;
            beamPool.FireAt(GameObjects.GetParentShip(transform.gameObject), gun.transform, gunFocalPoint, initalIntensity, beamColor);
        }

        private void ToggleFiringMode()
        {
            firingMode = firingMode == FiringMode.Single ? FiringMode.Dual : FiringMode.Single;
        }
    }
}