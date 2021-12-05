using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RandomEncounter : MonoBehaviour
{
    public GameObject playerGO;
    public CharState state;

    public float DistanceCap = 15f;
    float walkedDistance = 0f;
    float cumulativeTime = 0f;

    PlayerMovement player;
    private void Start()
    {
        player = playerGO.GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player.IsMoving)
        {
            cumulativeTime += Time.deltaTime;
            walkedDistance += player.moveSpeed * Time.deltaTime;
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
        //whatever
        //return Random.value * 0.5 + walkedDistance / DistanceCap > 1;
        return walkedDistance > DistanceCap;
    }

    void StartEncounter()
    {
        Debug.Log("RANDOM ENCOUNTER!");
        SceneManager.LoadScene("BattleScene");//, LoadSceneMode.Additive);
    }
}
