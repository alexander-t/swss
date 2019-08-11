using Core;
using Flying;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AI
{
    /*
     * This class will be merged with the regular TargetingComputer sometime. 
     * For this reason, it uses the naïve impplementations.
     */
    public class AITargetingComputer : MonoBehaviour
    {
        private Ship ship;
        private GameObject player;
        private Dictionary<string, Enemy> battlefield = new Dictionary<string, Enemy>();

        #region Unity lifecycle
        void Awake()
        {
            ship = GetComponent<Ship>();
            player = GameObject.Find(Constants.Player);
        }

        void Start()
        {
            StartCoroutine(UpdateEnemyDistanceMap());
        }
        #endregion

        public List<GameObject> GetTargetsWithinDistance(float distance) {
            return battlefield.Where(kv => kv.Value.Distance <= distance).Select(kv => kv.Value.GameObject).ToList();
        }

        private ShipFaction GetEnemyFaction()
        {
            if (ship.ShipFaction == ShipFaction.Neutral)
            {
                throw new InvalidOperationException("Can't determine enemy faction for neutral ship " + ship.name);
            }
            return ship.ShipFaction == ShipFaction.Alliance ? ShipFaction.Empire : ShipFaction.Alliance;
        }

        private void RebuildBattleField()
        {
            battlefield.Clear();
            ShipFaction enemyFaction = GetEnemyFaction();

            foreach (GameObject go in GameObject.FindGameObjectsWithTag(Constants.Tag_Ship))
            {
                if (go.GetComponent<Ship>().ShipFaction == enemyFaction)
                {
                    battlefield[go.name] = new Enemy()
                    {
                        GameObject = go,
                        Distance = Vector3.Distance(transform.position, go.transform.position)
                    };
                }
            }
/*
            string s = name + " > ";
            foreach (string enemyName in battlefield.Keys) {
                s += enemyName + "(" + battlefield[enemyName].Distance + ")";
            }
            Debug.Log(s);*/
        }

        IEnumerator UpdateEnemyDistanceMap()
        {
            while (true)
            {
                RebuildBattleField();
                yield return new WaitForSeconds(0.2f);
            }
        }

        private class Enemy
        {
            public GameObject GameObject { get; set; }
            public float Distance { get; set; }
        }
    }
}
