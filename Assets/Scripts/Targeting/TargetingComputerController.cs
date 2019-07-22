using Core;
using Flying;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Targeting
{
    public class TargetingComputerController : MonoBehaviour
    {
        [Tooltip("UI element that displays the name of the target.")]
        public Text targetNameText;

        [Tooltip("UI element that displays the target's hull points")]
        public Text targetHullText;

        [Tooltip("UI element that displays the target's shield points")]
        public Text targetShieldText;

        [Tooltip("UI element that displays the distance to the target")]
        public Text targetDistanceText;

        private TargetingComputer targetingComputer;
        private FollowingTargetCamera followingTargetCamera;

        // Target cycling
        private const float Cooldown = 0.2f;
        private float nextAcquisitionTime;

        private GameObject currentTarget;
        private float currentTargetDistance = 0;

        // Not used now
        //private Camera mainCamera;

        #region Unity lifecycle
        void Awake()
        {
            targetingComputer = TargetingComputer.Instance;
            followingTargetCamera = GetComponent<FollowingTargetCamera>();
        }

        void Start()
        {
            StartCoroutine(UpdateDistance());
            UpdateDisplay();
        }

        void Update()
        {
            if (Input.GetKey(KeyCode.T))
            {
                if (Time.time >= nextAcquisitionTime)
                {
                    targetingComputer.NextTarget();
                    UpdateDisplay();
                    nextAcquisitionTime = Time.time + Cooldown;
                }
            }
        }
        #endregion

        #region Events
        public void OnNewTarget(Ship ship)
        {
            /* Discard the player, which is also a ship and it's being registered upon scene startup.
             * Doing this StartsWith like this is a bit crude, but it allows the existence of prefabs starting with "Player - ".
             */
            if (!GameObjects.IsPlayer(ship.name))
            {
                TargetingComputer.Instance.AddTarget(ship);
                UpdateDisplay();
            }
        }

        public void OnEnemyHit()
        {
            UpdateDisplay();
        }

        public void OnEnemyDestroyed(string name)
        {
            targetingComputer.RemoveTargetByName(name);
            UpdateDisplay();
        }
        #endregion

        #region Class methods
        private void UpdateDisplay()
        {
            if (currentTarget != null)
            {
                currentTarget.BroadcastMessage("OnTargetted", false);
            }

            var targetable = targetingComputer.GetCurrentTarget();
            if (targetable != null)
            {
                currentTarget = GameObject.Find(targetable.Name);
                currentTarget.BroadcastMessage("OnTargetted", true);

                followingTargetCamera.target = currentTarget;
                targetNameText.text = targetable.Name;
                if (targetable.ShipFaction == ShipFaction.Alliance)
                {
                    targetNameText.color = Color.green;
                }
                else
                {
                    targetNameText.color = Color.white;
                }
                targetHullText.text = targetable.HullPoints + "";
                targetShieldText.text = targetable.ShieldPoints + "";
            }
            else
            {
                targetNameText.text = "";
                targetHullText.text = "";
                targetShieldText.text = "";
                targetDistanceText.text = "";
            }
        }

        // Coroutine to keep non-flickering update pace and to avoid over execution
        IEnumerator UpdateDistance()
        {
            while (true)
            {
                if (currentTarget != null)
                {
                    currentTargetDistance = Vector3.Distance(transform.position, currentTarget.transform.position);
                    targetDistanceText.text = string.Format("{0:0.0}", currentTargetDistance);
                }
                else
                {
                    targetDistanceText.text = "";
                }
                yield return new WaitForSeconds(0.1f);

            }
        }

        #endregion
    }
}