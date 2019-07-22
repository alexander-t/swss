using Flying;
using UnityEngine;

namespace Mission
{
    public class Demo : MonoBehaviour
    {
        public GameObject[] enemyShips;

        private readonly Vector3 SpawnPoint = new Vector3(100, 0, 100);
        private int rookieIndex = 1;

        void Awake()
        {
            EventManager.onShipDestroyed += OnDestroyed;
        }

        void Start()
        {
            Cursor.visible = false;
            SpawnXWing();
        }

        private void SpawnXWing()
        {
            var xWingPrefab = enemyShips[0];

            GameObject xWing = Instantiate(xWingPrefab, SpawnPoint, Quaternion.identity);
            xWing.name = "Rookie " + rookieIndex++;
            AIPilot aiPilot = xWing.GetComponent<AIPilot>();
            aiPilot.waypoints = new Transform[]
            {
                GameObject.Find("Waypoints/Waypoint 1").GetComponent<Transform>(),
                GameObject.Find("Waypoints/Waypoint 2").GetComponent<Transform>(),
                GameObject.Find("Waypoints/Waypoint 3").GetComponent<Transform>(),
                GameObject.Find("Waypoints/Waypoint 4").GetComponent<Transform>()
            };
        }

        #region Events
        public void OnDestroyed(string name)
        {
            if (name.StartsWith("Rookie"))
            {
                SpawnXWing();
            }
        }
        #endregion

    }
}