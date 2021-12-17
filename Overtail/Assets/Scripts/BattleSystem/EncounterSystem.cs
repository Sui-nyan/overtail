using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using Overtail.Battle;

namespace Overtail.Something {
    /// <summary>
    /// Class to generate a random encounter and transition into battle.
    /// Attach to any GameObject.
    /// </summary>
    public class EncounterSystem : MonoBehaviour
    {
        public UnityEngine.GameObject playerGO;
        public BattleSetupData setup;

        public float maxWalkingDistance = 10f;
        float walkedDistance = 0f;
        float cumulativeTime = 0f;

        public int minLevel = 1;
        public int maxLevel = 99;

        public List<EnemyEncounter> spawnableEnemies = new List<EnemyEncounter>();

        PlayerMovement playerMovement;
        private void Start()
        {
            playerMovement = playerGO.GetComponent<PlayerMovement>();
            //setup.playerStats = playerMovement.playerStatus; // get here or manually prepare beforehand
        }

        void Update()
        {
            if (playerMovement.IsMoving)
            {
                cumulativeTime += Time.deltaTime;
                walkedDistance += playerMovement.moveSpeed * Time.deltaTime;
                if (cumulativeTime > 1)
                {
                    cumulativeTime = 0;
                    if (encounterFormula())
                    {
                        walkedDistance = 0;
                        StartEncounter();
                    }
                }
            }
        }

        bool encounterFormula()
        {
            return walkedDistance > maxWalkingDistance; // return Random.value * 0.5 + walkedDistance / DistanceCap > 1;
        }

        /*IEnumerator*/
        void StartEncounter()
        {
            Debug.Log("<Random Encounter!>");
            SetRandomEnemy();
            SceneManager.LoadScene("BattleScene");//, LoadSceneMode.Additive);
        }

        void SetRandomEnemy()
        {
            /*        int rnd = Random.RandomWeighted(spawnableEnemies.Select(entry => entry.weightedProbability).ToArray());
                    Debug.Log("A wild " + spawnableEnemies[rnd].placeholderString.ToUpper() + " appeared");
                    //setup.enemyPrefab = spawnableEnemies[rnd].enemyPrefab;
                    var e = setup.enemyPrefab.GetComponent<BattleUnit>();
                    var encounter = spawnableEnemies[rnd];
                    e.name = encounter.placeholderString != "" ? encounter.placeholderString : e.name; // placeholder example
                    e.level = encounter.levelOverride > 0 ? encounter.levelOverride : UnityEngine.Random.Range(minLevel, maxLevel + 1);
              */
        }
    }

    [System.Serializable]
    public class EnemyEncounter
    {
        public string placeholderString;
        public UnityEngine.GameObject enemyPrefab;
        public int overrideLevel;
        public int weightedProbability;
    }

    public static class Random
    {
        public static int RandomWeighted(int[] weights)
        {
            foreach (int w in weights)
                if (w < 0) throw new System.Exception("Weights cannot be negative");


            if (weights.Length == 0 || weights.Sum() == 0)
                throw new System.Exception("Invalid array of weights");

            int delta = UnityEngine.Random.Range(0, weights.Sum());

            for (int i = 0; i < weights.Length; i++)
            {


                delta -= weights[i];
                if (delta < 0) return i;
            }

            throw new System.Exception("Unexpected result: i");
        }
    }
}