using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleUnit : MonoBehaviour
{
    public new string name;
    public int level;
    public int exp;

    public int attack;

    public int maxHP;
    public int currentHP;

    public bool TakeDamage(int dmg)
    {
        currentHP -= dmg;

        if (currentHP <= 0)
        {
            currentHP = 0;
            return true;
        }
        else
            return false;
    }

    public void Load(PlayerStats playerStatus)
    {
        this.name = playerStatus.name;
        this.level = playerStatus.level;
        this.exp = playerStatus.exp;

        this.attack = 0;

        this.maxHP = playerStatus.maxHP;
        this.currentHP = playerStatus.currentHP;
    }

    public void Save(PlayerStats playerStatus)
    {

        playerStatus.name = this.name;
        playerStatus.level = this.level + 1;
        playerStatus.exp = this.exp;

        // attack = 0;

        playerStatus.maxHP = this.maxHP;
        playerStatus.currentHP = this.currentHP;
    }
}
