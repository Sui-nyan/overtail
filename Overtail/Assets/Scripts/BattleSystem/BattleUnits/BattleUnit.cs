using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Overtail.Pending;
using UnityEngine;

namespace Overtail.Battle
{
    /// <summary>
    /// Main class for characters participating in combat.
    /// Main Interface to the battle system and vice versa.
    /// </summary>
    [DisallowMultipleComponent]
    public abstract class BattleUnit : MonoBehaviour, IBattleInteractable
    {
        public event Action<BattleUnit> StatusUpdated;

        [SerializeField] private string _displayName;
        [SerializeField] private int _level;

        // Set once on Loading only;
        private int _baseMaxHp;
        private int _baseAttack;
        private int _baseDefense;

        [SerializeField] private int _maxHp;
        [SerializeField] private int _attack;
        [SerializeField] private int _defense;

        [SerializeField] private int hp;

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

        public virtual int HP
        {
            get => hp;
            set
            {
                hp = Mathf.Clamp(value, 0, MaxHP);
                StatusUpdated?.Invoke(this);
            }
        }

        public virtual int MaxHP
        {
            get
            {
                if (!_statsInitialized) InitializeStats();
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
                if (!_statsInitialized) InitializeStats();
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
                if (!_statsInitialized) InitializeStats();
                return _defense;
            }
            set
            {
                _defense = value;
                StatusUpdated?.Invoke(this);
            }
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
                InitializeStats();
            }
        }

        public void AddStatusEffect(StatusEffect buff)
        {
            statusEffects.Add(buff);
            CalculateStat(buff.StatType);
        }

        public void SetBaseStats(int newMaxHp, int newAttack, int defense)
        {
            Debug.LogWarning(Name + "::" + MethodBase.GetCurrentMethod().Name);

            _baseMaxHp = newMaxHp;
            _baseAttack = newAttack;
            _baseDefense = defense;
            InitializeStats();
        }

        private void InitializeStats()
        {
            Debug.LogWarning(Name + "::" + MethodBase.GetCurrentMethod().Name);

            _statsInitialized = true;
            try
            {
                // Unity Inspector as fallback values
                if (_baseMaxHp == 0) _baseMaxHp = _maxHp;
                if (_baseAttack == 0) _baseAttack = _attack;
                if (_baseDefense == 0) _baseDefense = _defense;

                foreach (var type in (StatType[]) Enum.GetValues(typeof(StatType)))
                {
                    CalculateStat(type);
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                _statsInitialized = false;
            }

        }


        private void CalculateStat(StatType statType)
        {
            Debug.LogWarning($"{Name}::{MethodBase.GetCurrentMethod().Name}::{statType}");

            float percent = 0;
            int flat = 0;

            foreach (StatusEffect status in statusEffects.FindAll(s => s.StatType == statType))
            {
                if (status.ModifierType == ModifierType.PERCENTAGE)
                    percent += status.Value;

                if (status.ModifierType == ModifierType.FLAT)
                    flat += (int) status.Value;
            }

            
            switch (statType)
            {
                case StatType.MAXHP:
                    MaxHP = (int) (_baseMaxHp * (1 + percent) + flat);
                    break;
                case StatType.ATTACK:
                    Attack = (int) (_baseAttack * (1 + percent) + flat);
                    break;
                case StatType.DEFENSE:
                    Defense = (int) (_baseDefense * (1 + percent) + flat);
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

        public virtual IEnumerator OnGreeting(BattleSystem system)
        {
            yield break;
        }
        public virtual IEnumerator DoTurnLogic(BattleSystem system)
        {
            yield break;
        }

        public virtual IEnumerator OnVictory(BattleSystem system)
        {
            yield break;
        }

        public virtual IEnumerator OnDefeat(BattleSystem system)
        {
            yield break;
        }

        public virtual IEnumerator OnAttack(BattleSystem system)
        {
            yield break;
        }

        public virtual IEnumerator OnGetAttacked(BattleSystem system)
        {
            yield break;
        }

        public virtual IEnumerator OnFlirt(BattleSystem system)
        {
            yield break;
        }
        public virtual IEnumerator OnGetFlirted(BattleSystem system)
        {
            yield break;
        }

        public virtual IEnumerator OnBully(BattleSystem system)
        {
            yield break;
        }
        public virtual IEnumerator OnGetBullied(BattleSystem system)
        {
            yield break;
        }
        public virtual IEnumerator OnItemUse(BattleSystem system, ItemStack itemStack)
        {
            yield break;
        }

        public virtual IEnumerator OnEscape(BattleSystem system)
        {
            yield break;
        }

        public virtual IEnumerator OnOpponentEscapes(BattleSystem system)
        {
            yield break;
        }
    }
}