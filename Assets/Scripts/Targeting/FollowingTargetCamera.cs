using UnityEngine;

namespace Targeting
{
    /*
     * Positions a tracking camera so that it looks at a target from a constant angle,
     * while retaining the player's Y-orientation.
     */
    public class FollowingTargetCamera : MonoBehaviour
    {
        public GameObject player;
        [Tooltip("Target to look at; set dynamically by scripts")]
        public GameObject target;
        [Space(10)]
        public Camera trackingCamera;

        // TODO: Needs to be recalculated depending on the ship size!
        private readonly Vector3 Offset = new Vector3(20, 10, 10);

        void LateUpdate()
        {
            trackingCamera.transform.position = target.transform.position + Offset;
            trackingCamera.transform.LookAt(target.transform.position, player.transform.up);
        }
    }
}
