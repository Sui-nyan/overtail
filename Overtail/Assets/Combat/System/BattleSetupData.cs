using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;



[CreateAssetMenu(fileName = "BattleSetup", menuName = "Encounter/Battle Setup Object")]
// Data class containing information to setup the battle scene
public class BattleSetupData : ScriptableObject
{
    [SerializeField] public GameObject playerPrefab;
    [SerializeField] public GameObject enemyPrefab;
}
