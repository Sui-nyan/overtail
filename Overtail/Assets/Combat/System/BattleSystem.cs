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

    public BattleSetupData setup;

    public BattleHUD playerHUD;
    public BattleHUD enemyHUD;

    public Transform playerStation;
    public Transform enemyStation;

    BattleUnit playerUnit;
    BattleUnit enemyUnit;

    public GameObject DialogueBox_unused;
    public Text textBox;

    void Start()
    {
        state = BattleState.START;
        StartCoroutine(SetupBattle());
    }

    void UpdateHUD()
    {
        playerHUD.SetHUD(playerUnit);
        enemyHUD.SetHUD(enemyUnit);
    }
    IEnumerator SetupBattle()
    {
        // Instantiating should call respective GameObject.Start()
        GameObject playerObject = Instantiate(setup.playerPrefab, playerStation);
        playerUnit = playerObject.GetComponent<BattleUnit>();

        GameObject enemyGO = Instantiate(setup.enemyPrefab, enemyStation);
        enemyUnit = enemyGO.GetComponent<BattleUnit>();

        UpdateHUD();

        textBox.text = playerUnit.name + " vs. " + enemyUnit.name;
        yield return new WaitForSeconds(2f);
        // Replace with input

        state = BattleState.PLAYERTURN;
        StartCoroutine(PlayerTurn());
    }


    IEnumerator PlayerTurn()
    {
        textBox.text = "Your Turn";
        yield return new WaitForSeconds(0.1f);
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
            textBox.text = "You won.";
        else
            textBox.text = "You didnt win";

        SceneManager.LoadScene("SampleScene");
    }

    IEnumerator PlayerAttack()
    {
        Debug.Log("playerattack");
        // damage
        bool isDead = false;//enemyUnit.TakeDamage(playerUnit.attack);

        // enemyHUD.SetHP(enemyUnit.CurrentHealth);
        textBox.text = "Attacked!";
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



    public void _ENDBATTLE()
    {
        //playerUnit.Save(battleSetup.playerStats);
        EndBattle();
    }



    public void OnAttackButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;

        StartCoroutine(PlayerAttack());

    }
}
