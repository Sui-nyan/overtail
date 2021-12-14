using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Overtail.Deprecated
{
    public class _BattleSystem : MonoBehaviour
    {
        [System.Serializable]
        public class _BattleSetup
        {
            [SerializeField] public GameObject playerPrefab;
            [SerializeField] public GameObject enemyPrefab;

            public Transform playerStation;
            public Transform enemyStation;
        }

        public _BattleSetup setup;


        void Start()
        {
            SetupGameObjects();

        }

        void SetupGameObjects()
        {
            GameObject playerObject = Instantiate(setup.playerPrefab, setup.playerStation);

        }


        // Update is called once per frame
        void Update()
        {

        }
    }

}