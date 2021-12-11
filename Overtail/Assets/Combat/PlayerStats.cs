using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Persistent player object
 * Get all stats from this object
 * Make sure to keep it updated
 */
[CreateAssetMenu(fileName = "PlayerStats", menuName = "PlayerStats", order = 1)]
public class PlayerStats : ScriptableObject
{
    public new string name = "McWhopper";

    public int level = 1;
    public int exp;

    public int maxHP = 5;
    public int currentHP = 5;
    public Vector3 position;

    public void SetPosition(Vector3 vec)
    {
        position = vec;
    }

    public void AddExp(int expGain)
    {
        this.exp += expGain;

        // exp curve
        if (this.exp > 10)
        {
            this.exp -= 10;
            level++;
            // LevelUp Event
            this.AddExp(0);
        }
    }

    public void SetExp(int newExp)
    {
        this.exp = newExp;
        this.AddExp(0);
    }
}
