using Core;
using Flying;
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

        private readonly Vector3 ViewingDistanceSmall = new Vector3(5, 5, 5);
        private readonly Vector3 ViewingDistanceStandard = new Vector3(13, 13, 13);
        private readonly Vector3 ViewingDistanceCapital = new Vector3(90, 90, 90);

        void LateUpdate()
        {
            if (target == null) {
                return;
            }

            Ship ship = target.GetComponent<Ship>();
            Vector3 offset = ViewingDistanceStandard;
            switch (ship.shipData.Size)
            {
                case ShipSize.Small:
                    offset = ViewingDistanceSmall;
                    break;
                case ShipSize.Standard:
                    offset = ViewingDistanceStandard;
                    break;
                case ShipSize.Capital:
                    offset = ViewingDistanceCapital;
                    break;
            }
            trackingCamera.transform.position = target.transform.position + offset;
            trackingCamera.transform.LookAt(target.transform.position, player.transform.up);
        }
    }
}
