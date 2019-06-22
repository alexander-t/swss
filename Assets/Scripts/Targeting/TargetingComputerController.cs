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

        private new Camera camera;
        private GameObject currentTarget;
        private Rect targetBoundingBox = Rect.zero;

        #region Unity lifecycle
        void Awake()
        {
            targetingComputer = TargetingComputer.Instance;
            camera = GameObject.Find("Main Camera").GetComponent<Camera>();
        }

        void Start()
        {
            followingTargetCamera = GameObject.Find("Targeting Computer").GetComponent<FollowingTargetCamera>();
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
                }
                nextAcquisitionTime = Time.time + Cooldown;
            }
            targetBoundingBox = CalcualateTargetBoundingBox(currentTarget);
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
            var targetable = targetingComputer.GetCurrentTarget();
            currentTarget = GameObject.Find(targetable.Name);
            followingTargetCamera.target = currentTarget;
            targetNameText.text = targetable.Name;
            targetHullText.text = targetable.HullPoints + "";
            targetShieldText.text = targetable.ShieldPoints + "";
                        
        }
        #endregion

        // The contents of this method are borrowed from here: https://answers.unity.com/questions/726412/2d-target-reticle-for-3d-object.html
        private Rect CalcualateTargetBoundingBox(GameObject target) {
            if (!IsTargetVisible(target)) {
                return Rect.zero;
            }

            Bounds bounds = target.GetComponentInChildren<Renderer>().bounds;

            // Find bounds (cube) corners
            Vector3[] Corners3 = new Vector3[8];
            Corners3[0] = new Vector3(bounds.min.x, bounds.min.y, bounds.min.z);
            Corners3[1] = new Vector3(bounds.max.x, bounds.min.y, bounds.min.z);
            Corners3[2] = new Vector3(bounds.max.x, bounds.max.y, bounds.min.z);
            Corners3[3] = new Vector3(bounds.min.x, bounds.max.y, bounds.min.z);
            Corners3[4] = new Vector3(bounds.min.x, bounds.min.y, bounds.max.z);
            Corners3[5] = new Vector3(bounds.max.x, bounds.min.y, bounds.max.z);
            Corners3[6] = new Vector3(bounds.max.x, bounds.max.y, bounds.max.z);
            Corners3[7] = new Vector3(bounds.min.x, bounds.max.y, bounds.max.z);

            // Find furthest screen positions of the corners
            Vector2 Min = camera.WorldToScreenPoint(Corners3[0]);
            Vector2 Max = Min;
            for (int i = 1; i < Corners3.Length; i++)
            {
                Vector2 ScreenPos = camera.WorldToScreenPoint(Corners3[i]);
                Min.x = Mathf.Min(Min.x, ScreenPos.x);
                Min.y = Mathf.Min(Min.y, ScreenPos.y);
                Max.x = Mathf.Max(Max.x, ScreenPos.x);
                Max.y = Mathf.Max(Max.y, ScreenPos.y);
            }

            // Screen space is inverted and starts at the bottom
            Min.y = Screen.height - Min.y;
            Max.y = Screen.height - Max.y;
            float Height = Min.y - Max.y;
            return new Rect(Min.x, Max.y, Max.x - Min.x, Height);
        }

        private bool IsTargetVisible(GameObject target) {
            Vector3 sp = camera.WorldToViewportPoint(target.transform.position);
            return sp.z > 0 && sp.x > 0 && sp.x < 1 && sp.y > 0 && sp.y < 1;
        }

        private void OnGUI()
        {
            Debug.Log(GUI.depth);
            GUI.Box(targetBoundingBox, "");
        }
    }
}