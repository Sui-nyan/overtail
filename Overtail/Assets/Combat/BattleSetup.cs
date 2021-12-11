using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;



[CreateAssetMenu(fileName = "BattleSetup", menuName = "Encounter/Battle Setup Object")]
// Data class containing information to setup the battle scene
public class BattleSetup : ScriptableObject
{
    public PlayerStats playerStats;

    public GameObject playerPrefab;
    public GameObject enemyPrefab;    
}


