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

    public CharState playerState;
    public GameObject playerPrefab;
    public GameObject enemyPrefab;

    public Transform playerStation;
    public Transform enemyStation;

    Unit playerUnit;
    Unit enemyUnit;

    public Text dialogue;

    public BattleHUD playerHUD;
    public BattleHUD enemyHUD;

    public CharState characterState;

    // Start is called before the first frame update
    void Start()
    {
        state = BattleState.START;
        Debug.Log(playerState.position);
        StartCoroutine(SetupBattle());
    }

    IEnumerator SetupBattle()
    {
        GameObject playerGO = Instantiate(playerPrefab, playerStation);
        playerUnit = playerGO.GetComponent<Unit>();

        {
            playerUnit.currentHP = playerState.currentHP;
            playerUnit.maxHP = playerState.maxHP;
            playerUnit.level = playerState.level;
        }

        GameObject enemyGO = Instantiate(enemyPrefab, enemyStation);
        enemyUnit = enemyGO.GetComponent<Unit>();

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
