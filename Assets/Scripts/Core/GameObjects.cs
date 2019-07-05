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
         * Return a subcomponent's parent tagged "Ship" or the component itself if it's the parent
         */ 
        public static GameObject GetParentShip(GameObject go)
        {
            if (go.CompareTag(Constants.Tag_Ship))
            {
                return go;
            }

            GameObject parent = go.transform.parent.gameObject;

            if (parent.CompareTag(Constants.Tag_Ship))
            {
                return parent;
            }

            // This might happen if objects are deeply nested, but let's deal with it then.
            throw new ArgumentException("Couldn't find parent ship of " + go.name);
        }
    }
}
