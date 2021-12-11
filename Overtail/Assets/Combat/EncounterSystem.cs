using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EncounterSystem : MonoBehaviour
{
    public GameObject playerGO;
    public BattleSetup setup;
    public PlayerStats PlayerInfo;
    public GameObject enemyPre;

    public float DistanceCap = 10f;
    float walkedDistance = 0f;
    float cumulativeTime = 0f;

    public List<GameObject> spawnables = new List<GameObject>();

    PlayerMovement playerMovement;
    private void Start()
    {
        playerMovement = playerGO.GetComponent<PlayerMovement>();
        setup.playerStats = playerMovement.playerStatus;
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
                if(encounterFormula())
                {
                    walkedDistance = 0;
                    StartEncounter();
                }
            }
        }
    }

    bool encounterFormula()
    {
        // whatever
        return walkedDistance > DistanceCap; // return Random.value * 0.5 + walkedDistance / DistanceCap > 1;
    }

    void StartEncounter()
    {
        Debug.Log("RANDOM ENCOUNTER!");
        SetRandomEnemy();        
        SceneManager.LoadScene("BattleScene");//, LoadSceneMode.Additive);
    }

    void SetRandomEnemy()
    {
        int rnd = Random.Range(0, spawnables.Count);
        setup.enemyPrefab = spawnables[rnd];
    }
}
