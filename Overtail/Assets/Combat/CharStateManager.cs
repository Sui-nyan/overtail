using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class CharStateManager : MonoBehaviour
{
    public CharState playerState;
    public CharState enemyState;

    #region Singleton
    public static CharStateManager instance;
    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }
    #endregion

    public Animator transition; // TODO
    private void prepareBattle() // where?
    {
        playerState.position[0] = this.transform.position.x;

        enemyState.name = "TheRock";
        enemyState.maxHP = 30;
        enemyState.currentHP = 25;
    }

    public void LoadBattle(string levelName)
    {
        StartCoroutine(LoadNamedLevel(levelName));
    }

    
    IEnumerator LoadNamedLevel(string levelName)
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(1f); //transitionTime
        SceneManager.LoadScene(levelName);

        transition.SetTrigger("End");
    }

}
