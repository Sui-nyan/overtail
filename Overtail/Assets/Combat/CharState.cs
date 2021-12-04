using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharStateData", menuName = "CharState/Something", order = 1)]
public class CharState : ScriptableObject
{
    public string name;
    public int level;
    public int maxHP;
    public int currentHP;
    public float[] position = new float[2];
    public GameObject characterGO; //?


}
