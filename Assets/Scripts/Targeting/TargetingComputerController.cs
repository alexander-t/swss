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
        
        private TargetingComputer targetingComputer;
        private FollowingTargetCamera followingTargetCamera;
        private const float Cooldown = 0.1f;
        private float nextAcquisitionTime;

        #region Unity lifecycle
        void Awake()
        {
            targetingComputer = TargetingComputer.Instance;
        }

        void Start()
        {
            followingTargetCamera = GameObject.Find("Targeting Computer").GetComponent<FollowingTargetCamera>();
            UpdateDisplay();        
        }

        void LateUpdate()
        {
            if (Input.GetKey(KeyCode.T))
            {
                if (Time.time >= nextAcquisitionTime)
                {
                    targetingComputer.NextTarget();
                    UpdateDisplay();
                }
                nextAcquisitionTime = Time.time + Cooldown;
            }
        }
        #endregion

        #region Events
        public void OnEnemyHit()
        {
            UpdateDisplay();
        }

        public void OnEnemyDestroyed(string name) {
            targetingComputer.RemoveTargetByName(name);
            UpdateDisplay();
        }
        #endregion

        #region Class methods
        private void UpdateDisplay()
        {
            var currentTarget = targetingComputer.GetCurrentTarget();
            followingTargetCamera.target = GameObject.Find(currentTarget.Name);
            targetNameText.text = currentTarget.Name;
            targetHullText.text = currentTarget.HullPoints + "";
            targetShieldText.text = currentTarget.ShieldPoints + "";
        }
        #endregion

    }
}