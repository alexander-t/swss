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
            if (go.CompareTag(Constants.Tag_Ship) || Constants.Player.Equals(Constants.Player))
            {
                return go;
            }

            Transform parent = go.transform.parent;

            if (parent != null && parent.gameObject.CompareTag(Constants.Tag_Ship))
            {
                return parent.gameObject;
            }

            // This might happen if objects are deeply nested, but let's deal with it then.
            throw new ArgumentException("Couldn't find parent ship of " + go.name);
        }
    }
}
