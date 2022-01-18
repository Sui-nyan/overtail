using System.Collections.Generic;
using System.Linq;
using Overtail.Battle.Entity;

namespace Overtail.Battle.Encounter
{
    [System.Serializable]
    public class EnemyEncounter
    {
        public EnemyEntity enemy;
        public int weightedProbability = 0;
        public bool randomizeLevel = false;

        public EnemyEncounter(EnemyEntity e)
        {
            this.enemy = e;
        }

        public static EnemyEncounter GetRandom(List<EnemyEncounter> pool) => GetRandom(pool.ToArray());
        public static EnemyEncounter GetRandom(EnemyEncounter[] pool)
        {
            var weights = pool.Select(entry => entry.weightedProbability).ToArray();
            var index = GetRandomIndex(weights);
            return pool[index];
        }

        public static int GetRandomIndex(int[] weights)
        {
            foreach (int w in weights)
                if (w < 0)
                    throw new System.Exception("Weights cannot be negative");


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
