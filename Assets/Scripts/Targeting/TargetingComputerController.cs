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

        [Tooltip("UI element that displays the contents of the target")]
        public Text targetContentsText;

        [Space(7)]

        [Tooltip("Used for playing bleeps")]
        public AudioSource audioSource;

        private TargetingComputer targetingComputer;
        private FollowingTargetCamera followingTargetCamera;

        // Target cycling
        private const float Cooldown = 0.2f;
        private float nextAcquisitionTime;

        private GameObject currentTarget;
        private float currentTargetDistance = 0;

        #region Unity lifecycle
        void Awake()
        {
            targetingComputer = new TargetingComputer();
            followingTargetCamera = GetComponent<FollowingTargetCamera>();

            EventManager.onShipDestroyed += OnShipDestroyed;
            EventManager.onShipHit += OnShipHit;
            EventManager.onNewShipEntered += OnNewShipEntered;
            EventManager.onShipInspected += OnShipHit; // Just update the display
        }

        void Start()
        {
            StartCoroutine(UpdateDistance());
            UpdateDisplay();
        }

        void OnDestroy()
        {
            EventManager.onShipDestroyed -= OnShipDestroyed;
            EventManager.onShipHit -= OnShipHit;
            EventManager.onNewShipEntered -= OnNewShipEntered;
            EventManager.onShipInspected -= OnShipHit;
        }

        void Update()
        {
            bool targetUpdated = false;
            if (Input.GetKeyUp(KeyCode.T))
            {
                targetingComputer.NextTarget();
                targetUpdated = targetingComputer.GetCurrentTarget() != null;
            }
            else if (Input.GetKeyUp(KeyCode.Y))
            {
                targetingComputer.PreviousTarget();
                targetUpdated = targetingComputer.GetCurrentTarget() != null;
            }

            if (targetUpdated)
            {
                audioSource.Play();
                UpdateDisplay();
            }
        }
        #endregion


        #region Events
        private void OnNewShipEntered(Ship ship)
        {
            if (!GameObjects.IsPlayer(ship))
            {
                targetingComputer.AddTarget(ship);
                UpdateDisplay();
            }
        }

        private void OnShipHit(string name)
        {
            UpdateDisplay();
        }

        private void OnShipDestroyed(string name)
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

            Targettable targetable = targetingComputer.GetCurrentTarget();
            if (targetable != null)
            {
                currentTarget = GameObject.Find(targetable.Name);
                currentTarget.BroadcastMessage("OnTargetted", true);

                followingTargetCamera.target = currentTarget;
                targetNameText.text = targetable.Name;
                if (targetable.ShipFaction == ShipFaction.Alliance)
                {
                    targetNameText.color = Color.green;
                } else if (targetable.ShipFaction == ShipFaction.Empire)
                {
                    targetNameText.color = Color.red;
                }
                else
                {
                    targetNameText.color = Color.white;
                }
                targetHullText.text = targetable.HullPoints + "%";
                targetShieldText.text = targetable.ShieldPoints + "%";
                targetContentsText.text = targetable.Contents;
            }
            else
            {
                targetNameText.text = "";
                targetHullText.text = "";
                targetShieldText.text = "";
                targetDistanceText.text = "";
                targetContentsText.text = "";
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