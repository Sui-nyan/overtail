using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overtail.Battle
{
    /// <summary>
    /// Main class for characters participating in combat.
    /// Main Interface to the battle system and vice versa.
    /// </summary>
    public abstract class BattleUnit : MonoBehaviour, IBattleInteractable
    {
        public event Action<BattleUnit> StatusUpdate;

        [SerializeField] protected new string name;

        [SerializeField] protected int level;

        [Header("Combat Stats (Unbuffed)")]
        [SerializeField] protected int hp;
        [SerializeField] protected int maxHp;
        [SerializeField] protected int attack;
        [SerializeField] protected int defense;

        [SerializeField] protected List<StatusEffect> statusEffects = new List<StatusEffect>();

        public List<StatusEffect> StatusEffects => statusEffects;
        public virtual string Name => name;
        public virtual int Level => level;
        public virtual int HP
        {
            get => hp;
            set
            {
                hp = Mathf.Clamp(value, 0, MaxHP);
                StatusUpdate.Invoke(this);
            }
        }
        public virtual int MaxHP => GetStat(StatType.MAXHP);
        public virtual int Attack => GetStat(StatType.ATTACK);
        public virtual int Defense => GetStat(StatType.DEFENSE);

        protected virtual int GetStat(StatType statType)
        {
            float percent = 0;
            int flat = 0;

            foreach (StatusEffect status in statusEffects.FindAll(s => s.StatType == statType))
            {
                if (status.ModifierType == ModifierType.PERCENTAGE)
                    percent += status.Value;

                if (status.ModifierType == ModifierType.FLAT)
                    flat += (int)status.Value;
            }

            switch (statType)
            {
                case StatType.MAXHP:
                    return (int)(maxHp * (1 + percent) + flat);
                case StatType.ATTACK:
                    return (int)(attack * (1 + percent) + flat);
                case StatType.DEFENSE:
                    return (int)(defense * (1 + percent) + flat);
                default:
                    throw new System.ArgumentException();
            }
        }

        public virtual IEnumerator DoTurn(BattleSystem system)
        {
            yield break;
        }

        public virtual IEnumerator OnDefeat(BattleSystem system)
        {
            yield break;
        }

        public virtual IEnumerator OnVictory(BattleSystem system)
        {
            yield break;
        }


        public void InflictStatus(StatusEffect buff)
        {
            statusEffects.Add(buff);
            TurnUpdate(0);
        }

        public void TurnUpdate(int turns)
        {
            foreach (StatusEffect b in statusEffects.FindAll(s => s.FreezeDuration == false))
            {
                b.Duration -= turns;
            }

            statusEffects.RemoveAll(s => s.FreezeDuration == false && s.Duration <= 0);
        }

        public void TurnUpdate()
        {
            TurnUpdate(1);
        }
    }
}
