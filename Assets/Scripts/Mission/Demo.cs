using Targeting;
using Flying;
using UnityEngine;

namespace Mission
{
    public class Demo : MonoBehaviour
    {
        public GameObject[] enemyShips;

        private readonly Vector3 SpawnPoint = new Vector3(100, 0, 100);
        private int rookieIndex = 1;

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
                GameObject.Find("Buoys/A-122").GetComponent<Transform>(),
                GameObject.Find("Buoys/B-54").GetComponent<Transform>(),
                GameObject.Find("Buoys/B-55").GetComponent<Transform>(),
                GameObject.Find("Buoys/A-121").GetComponent<Transform>()
            };
        }

        #region Events
        public void OnEnemyDestroyed(string name)
        {
            TargetingComputer.Instance.RemoveTargetByName(name);
            GameObject.Find("Targeting Computer").SendMessage("UpdateDisplay");
            SpawnXWing();
        }
        #endregion

    }
}