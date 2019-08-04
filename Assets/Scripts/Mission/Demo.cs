using Core;
using Flying;
using UnityEngine;

namespace Mission
{
    public class Demo : MonoBehaviour
    {
        public GameObject[] enemyShips;
        
        private readonly Vector3 SpawnPoint = new Vector3(100, 0, 100);
        private int rookieIndex = 1;

        private GameObject player;
        private GameObject xWing;

        void Awake()
        {
            player = GameObject.Find(Constants.Player);
            
            EventManager.onShipDestroyed += OnShipDestroyed;
        }

        void Start()
        {
            Cursor.visible = false;
            SpawnXWing();
        }

        void Update()
        {
            if (Input.GetKey(KeyCode.P))
            {
                AIPilot aiPilot = xWing.GetComponent<AIPilot>();
                aiPilot.Attack(player);

            }
        }

        void OnDestroy()
        {
            EventManager.onShipDestroyed -= OnShipDestroyed;
        }

        private void SpawnXWing()
        {
            var xWingPrefab = enemyShips[0];

            xWing = Instantiate(xWingPrefab, SpawnPoint, Quaternion.identity);
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
        private void OnShipDestroyed(string name)
        {
            if (name.StartsWith("Rookie"))
            {
                SpawnXWing();
            }
        }
        #endregion

    }
}