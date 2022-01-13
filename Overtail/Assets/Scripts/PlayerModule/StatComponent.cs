using System;
using System.Collections.Generic;
using Overtail.Battle.Entity;
using Overtail.Battle;
using UnityEditor;
using UnityEngine;

namespace Overtail.PlayerModule
{
    public class StatComponent : MonoBehaviour
    {


        [Header("Base Stats")]
        [SerializeField] private int _baseMaxHp;
        [SerializeField] private int _baseAttack;
        [SerializeField] private int _baseDefense;

        [Header("Scaled Stats")]
        public int MaxHP;
        public int Attack;
        public int Defense;

        [Header("Other")]
        public int Charm;
        public int Level;
        public int Experience;
        public int HP;

        public List<StatusEffect> StatusEffects = new List<StatusEffect>();
        public int ExpLevelUp() => ExpLevelUp(Level);
        public int ScaledMaxHP(int level) => ScaledMaxHP(level, _baseMaxHp);
        public int ScaledAttack(int level) => ScaledOtherStat(level, _baseAttack);
        public int ScaledDefense(int level) => ScaledOtherStat(level, _baseDefense);

        void Awake()
        {
            RecalcStats();
        }

        public void RecalcStats()
        {
            MaxHP = ScaledMaxHP(Level);
            Attack = ScaledAttack(Level);

            Defense = ScaledDefense(Level);
        }

        public static int ExpLevelUp(int level)
        {
            return Mathf.CeilToInt((float) (0.8f * Math.Pow(level, 3)));
        }

        public static int ScaledMaxHP(int level, int baseMaxHp)
        {
            return Mathf.CeilToInt((baseMaxHp * 2f) * (level / 100f) + level + 10);
        }

        public static int ScaledOtherStat(int level, int baseValue)
        {
            return Mathf.CeilToInt((baseValue * 2f) * (level / 100f) + 5);
        }
    }
}