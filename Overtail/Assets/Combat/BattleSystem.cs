using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST }

// Credits to Brackeys - https://www.youtube.com/watch?v=_1pz_ohupPs 
public class BattleSystem : MonoBehaviour
{
    BattleState state;

    public BattleSetup battleSetup;

    public GameObject playerPrefab_unused;
    public GameObject enemyPrefab_unused;

    public BattleHUD playerHUD;
    public BattleHUD enemyHUD;

    public Transform playerStation;
    public Transform enemyStation;

    BattleUnit playerUnit;
    BattleUnit enemyUnit;

    public Text dialogue;

    void Start()
    {
        state = BattleState.START;
        StartCoroutine(SetupBattle());
    }

    IEnumerator SetupBattle()
    {
        GameObject playerGO = Instantiate(battleSetup.playerPrefab, playerStation);
        playerUnit = playerGO.GetComponent<BattleUnit>();

        playerUnit.Load(battleSetup.playerStats);

        GameObject enemyGO = Instantiate(battleSetup.enemyPrefab, enemyStation);
        enemyUnit = enemyGO.GetComponent<BattleUnit>();

        dialogue.text = playerUnit.name + " vs. " + enemyUnit.name;
        playerHUD.SetHUD(playerUnit);
        enemyHUD.SetHUD(enemyUnit);

        yield return new WaitForSeconds(2f);
        // Replace with input

        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }

    IEnumerator PlayerAttack()
    {
        Debug.Log("playerattack");
        // damage
        bool isDead = enemyUnit.TakeDamage(playerUnit.attack);

        enemyHUD.SetHP(enemyUnit.currentHP);
        dialogue.text = "Attacked!";
        state = BattleState.ENEMYTURN;

        yield return new WaitForSeconds(2f);

        if (isDead)
        {
            state = BattleState.WON;
            EndBattle();
        }

        if (state == BattleState.ENEMYTURN)
        {
            //EnemyTurn();
        }
    }

    IEnumerator EnemyTurn()
    {
        //...
        yield return new WaitForSeconds(2f);
        if (false) //player is dead, NEVER WHAHAHAAHAHA, GODMODE ACTIVATED
        {
            state = BattleState.LOST;
            EndBattle();
        }

        if (state == BattleState.ENEMYTURN)
        {
            PlayerTurn();
        }
    }

    void EndBattle()
    {
        if (state == BattleState.WON)
            dialogue.text = "You won.";
        else
            dialogue.text = "You didnt win";

        SceneManager.LoadScene("SampleScene");
    }

    public void _ENDBATTLE()
    {
        playerUnit.Save(battleSetup.playerStats);
        EndBattle();
    }

    void PlayerTurn()
    {
        dialogue.text = "Your Turn";
    }

    public void OnAttackButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;

        StartCoroutine(PlayerAttack());

    }
}
