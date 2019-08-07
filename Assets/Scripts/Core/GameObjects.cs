using Flying;
using System;
using UnityEngine;


namespace Core
{
    public class GameObjects
    {
        private GameObjects()
        {
        }

        /*
         * Return a subcomponent's parent tagged "Ship" or the component itself if it's the parent.
         * The player is also considered a parent ship.
         */
        public static GameObject GetParentShip(GameObject go)
        {
            if (go.CompareTag(Constants.Tag_Ship) || IsPlayer(go.name))
            {
                return go;
            }

            Transform parent = go.transform.parent;
            while (parent != null)
            {
                if (parent.gameObject.CompareTag(Constants.Tag_Ship))
                {
                    return parent.gameObject;
                }
                parent = parent.parent;
            }

            throw new ArgumentException("Couldn't find parent ship of " + go.name);
        }

        public static bool IsPlayer(string name)
        {
            return name != null && name.StartsWith(Constants.Player);
        }

        public static bool IsPlayer(Ship ship)
        {
            return IsPlayer(ship.gameObject.transform.parent?.name);
        }

        public static bool IsPlayer(GameObject go)
        {
            return IsPlayer(go.transform.parent?.name);
        }

        public static bool IsBeam(Collider collider)
        {
            return collider.CompareTag(Constants.Tag_LaserBeam);
        }

        public static bool IsShip(Collider collider)
        {
            return collider.CompareTag(Constants.Tag_Ship);
        }

        public static bool IsAsteroid(Collider collider)
        {
            return collider.CompareTag(Constants.Tag_Asteroid);
        }

        public static bool IsThreatArea(Collider collider)
        {
            return collider.CompareTag(Constants.Tag_ThreatArea);
        }

        public static bool IsShieldSphere(Collider collider)
        {
            return collider.CompareTag(Constants.Tag_ShieldSpehere);
        }
    }
}
