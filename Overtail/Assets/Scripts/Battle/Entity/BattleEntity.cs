using Overtail.Items;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overtail.Battle.Entity
{
    /// <summary>
    /// Main class for characters participating in combat.
    /// Main Interface to the battle system and vice versa.
    /// </summary>
    [DisallowMultipleComponent]
    public abstract class BattleEntity : MonoBehaviour, IBattleInteractable
    {
        public event Action<BattleEntity> StatusUpdated;

        [SerializeField] private string _displayName;
        [SerializeField] private int _level;
        [SerializeField] private int _experience;

        // Set once on Loading only;
        private int _baseMaxHp;
        private int _baseAttack;
        private int _baseDefense;

        [SerializeField] private int _maxHp;
        [SerializeField] private int _attack;
        [SerializeField] private int _defense;

        [SerializeField] private int _hp;

        private bool _statsInitialized = false;

        [SerializeField] protected List<StatusEffect> statusEffects = new List<StatusEffect>();

        // Properties


        public virtual string Name
        {
            get => _displayName;
            set
            {
                _displayName = value;
                name = value;
                StatusUpdated?.Invoke(this);
            }
        }

        public virtual int Level
        {
            get => _level;
            set
            {
                _level = value;
                StatusUpdated?.Invoke(this);
            }
        }
        public int Experience
        {
            get => _experience;
            set => _experience = value;
        }

        public virtual int HP
        {
            get => _hp;
            set
            {
                _hp = Mathf.Clamp(value, 0, MaxHP);
                StatusUpdated?.Invoke(this);
            }
        }

        public virtual int MaxHP
        {
            get
            {
                if (!_statsInitialized) CalculateFinalStats();
                return _maxHp;
            }
            set
            {
                _maxHp = value;
                StatusUpdated?.Invoke(this);
            }
        }

        public virtual int Attack
        {
            get
            {
                if (!_statsInitialized) CalculateFinalStats();
                return _attack;
            }
            set
            {
                _attack = value;
                StatusUpdated?.Invoke(this);
            }
        }

        public virtual int Defense
        {
            get
            {
                if (!_statsInitialized) CalculateFinalStats();
                return _defense;
            }
            set
            {
                _defense = value;
                StatusUpdated?.Invoke(this);
            }
        }

        protected void SetAll(string displayName, int level, int baseMaxHp, int baseAttack, int baseDefense, int hp, List<StatusEffect> statusEffects)
        {
            Name = displayName;
            Level = level;
            _baseMaxHp = baseMaxHp;
            _baseAttack = baseAttack;
            _baseDefense = baseDefense;

            CalculateStat(StatType.MAXHP);
            this.HP = hp;
        }

        /// <summary>
        /// Returns a copy.<br/> Use <see cref="AddStatusEffect"/> or <br/>set a new one instead.
        /// </summary>
        public List<StatusEffect> StatusEffects
        {
            get => new List<StatusEffect>(statusEffects);
            set
            {
                statusEffects = value;
                CalculateFinalStats();
            }
        }

        public void AddStatusEffect(StatusEffect buff)
        {
            statusEffects.Add(buff);
            CalculateStat(buff.StatType);
        }

        public void SetBaseStats(int newMaxHp, int newAttack, int defense)
        {
            // Debug.LogWarning(Name + "::" + MethodBase.GetCurrentMethod().Name);

            _baseMaxHp = newMaxHp;
            _baseAttack = newAttack;
            _baseDefense = defense;
            CalculateFinalStats();
        }

        private void CalculateFinalStats()
        {
            // Debug.LogWarning(Name + "::" + MethodBase.GetCurrentMethod().Name);

            _statsInitialized = true;
            try
            {
                // Unity Inspector as fallback values
                if (_baseMaxHp == 0) _baseMaxHp = _maxHp;
                if (_baseAttack == 0) _baseAttack = _attack;
                if (_baseDefense == 0) _baseDefense = _defense;

                foreach (var type in (StatType[])Enum.GetValues(typeof(StatType)))
                {
                    CalculateStat(type);
                }
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogError(e);
                _statsInitialized = false;
            }

        }


        private void CalculateStat(StatType statType)
        {
            // Debug.LogWarning($"{Name}::{MethodBase.GetCurrentMethod().Name}::{statType}");

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
                    MaxHP = (int)(_baseMaxHp * (1 + percent) + flat);
                    break;
                case StatType.ATTACK:
                    Attack = (int)(_baseAttack * (1 + percent) + flat);
                    break;
                case StatType.DEFENSE:
                    Defense = (int)(_baseDefense * (1 + percent) + flat);
                    break;
                default:
                    throw new System.ArgumentException("Unknown Stat Type");
            }
        }

        public void TurnUpdate(int turns = 1)
        {
            HashSet<StatType> changed = new HashSet<StatType>();

            foreach (StatusEffect b in statusEffects.FindAll(s => s.FreezeDuration == false))
            {
                b.Duration -= turns;
            }

            statusEffects.RemoveAll(s =>
            {
                if (s.FreezeDuration == false && s.Duration <= 0)
                {
                    changed.Add(s.StatType);
                    return true;
                }

                return false;
            });

            foreach (var type in changed)
            {
                CalculateStat(type);
            }
        }

        /// <summary>
        /// What the entity says on combat start
        /// </summary>
        /// <param name="system"></param>
        /// <returns></returns>
        public virtual IEnumerator OnGreeting(BattleSystem system)
        {
            yield break;
        }

        /// <summary>
        /// Complete turn logic. Gets called when its their respective turn
        /// </summary>
        /// <param name="system"></param>
        /// <returns></returns>
        public virtual IEnumerator DoTurnLogic(BattleSystem system)
        {
            yield break;
        }

        /// <summary>
        /// Called when this entity wins a battle
        /// </summary>
        /// <param name="system"></param>
        /// <returns></returns>
        public virtual IEnumerator OnVictory(BattleSystem system)
        {
            yield break;
        }

        /// <summary>
        /// Called when this entity loses a battle
        /// </summary>
        /// <param name="system"></param>
        /// <returns></returns>
        public virtual IEnumerator OnDefeat(BattleSystem system)
        {
            yield break;
        }

        /// <summary>
        /// Called/Call this when this entity attacks the enemy.
        /// Include damage calculation here
        /// </summary>
        /// <param name="system"></param>
        /// <returns></returns>
        public virtual IEnumerator OnAttack(BattleSystem system)
        {
            yield break;
        }

        /// <summary>
        /// Called when this entity is attacked.
        /// Reaction stuff mostly. Don't do damage calculation!
        /// </summary>
        /// <param name="system"></param>
        /// <returns></returns>
        public virtual IEnumerator OnGetAttacked(BattleSystem system)
        {
            yield break;
        }

        /// <summary>
        /// Called/call this to Flirt with the opponent
        /// </summary>
        /// <param name="system"></param>
        /// <returns></returns>
        public virtual IEnumerator OnFlirt(BattleSystem system)
        {
            yield break;
        }

        /// <summary>
        /// Call this after opponent uses Flirt on entity
        /// </summary>
        /// <param name="system"></param>
        /// <returns></returns>
        public virtual IEnumerator OnGetFlirted(BattleSystem system)
        {
            yield break;
        }

        /// <summary>
        /// Called/call this to bully opponent
        /// </summary>
        /// <param name="system"></param>
        /// <returns></returns>
        public virtual IEnumerator OnBully(BattleSystem system)
        {
            yield break;
        }

        /// <summary>
        /// Called/call this after opponent uses bully()
        /// </summary>
        /// <param name="system"></param>
        /// <returns></returns>
        public virtual IEnumerator OnGetBullied(BattleSystem system)
        {
            yield break;
        }

        /// <summary>
        /// What happens when an item has been selected and is going to be used
        /// </summary>
        /// <param name="system"></param>
        /// <param name="itemStack"></param>
        /// <returns></returns>
        public virtual IEnumerator OnItemUse(BattleSystem system, ItemStack itemStack)
        {
            yield break;
        }

        /// <summary>
        /// Called when entity is going to escape
        /// </summary>
        /// <param name="system"></param>
        /// <returns></returns>
        public virtual IEnumerator OnEscape(BattleSystem system)
        {
            yield break;
        }

        /// <summary>
        /// Call this after opponent's <see cref="OnEscape"/>
        /// </summary>
        /// <param name="system"></param>
        /// <returns></returns>
        public virtual IEnumerator OnOpponentEscapes(BattleSystem system)
        {
            yield break;
        }
    }
}
