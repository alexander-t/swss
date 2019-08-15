using Core;
using Flying;
using System.Collections;
using UnityEngine;

namespace Mission
{
    public class Demo : MonoBehaviour
    {
        public GameObject xWingPrefab;
        public Transform xWingSpawnPoint;

        private int rookieIndex = 1;

        private GameObject player;
        private GameObject xWing;
        private GameObject tieFighter;

        void Awake()
        {
            player = GameObject.Find(Constants.Player);
            tieFighter = GameObject.Find("T/F buoy killer");

            EventManager.onShipDestroyed += OnShipDestroyed;
        }

        void Start()
        {
            Cursor.visible = false;
            SpawnXWing();
            StartCoroutine(AttackB54());
        }

        void OnDestroy()
        {
            EventManager.onShipDestroyed -= OnShipDestroyed;
        }

        private void SpawnXWing()
        {
            xWing = Instantiate(xWingPrefab, xWingSpawnPoint.position, Quaternion.identity);
            xWing.transform.LookAt(GameObject.Find("Waypoints/Waypoint 1").transform);
            xWing.name = "Rookie " + rookieIndex++;
            AIPilot aiPilot = xWing.GetComponent<AIPilot>();
            aiPilot.waypoints = new Transform[]
            {
                GameObject.Find("Waypoints/Waypoint 1").transform,
                GameObject.Find("Waypoints/Waypoint 2").transform,
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

        private IEnumerator AttackB54()
        {
            // Stupid, but needs to be run with a delay because of the special forcing of Attack() and default behavior initialization
            yield return new WaitForSeconds(1);
            tieFighter.GetComponent<AIPilot>().Attack(GameObject.Find("Buoys/B-54"));
        }
    }
}