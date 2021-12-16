using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overtail.Battle
{
    /// <summary>
    /// Buffs and other possible modifiers to characters.
    /// Duration in turns.
    /// </summary>
    [System.Serializable]
    public class StatusEffect
    {
        [SerializeField] private string name;
        [SerializeField] private StatType statType;
        [SerializeField] private ModifierType modifierType;
        [SerializeField] private float value;
        [SerializeField] private int duration;
        [SerializeField] private bool freezeDuration;

        public string Name => name;
        public StatType StatType => statType;
        public ModifierType ModifierType => modifierType;

        public int Duration
        {
            get => duration;
            set => duration = Mathf.Max(0, value);
        }
        public float Value => value;
        public bool FreezeDuration => freezeDuration;
        public void SetFreeze(bool b)
        {
            freezeDuration = b;
        }

        public StatusEffect(string name, StatType statType, ModifierType modifierType, float value, int duration, bool freezeDuration)
        {
            this.name = name;
            this.statType = statType;
            this.modifierType = modifierType;
            this.value = value;
            this.duration = duration;
            this.freezeDuration = freezeDuration;
        }
        public StatusEffect(StatusEffect s)
        {
            this.name = s.name;
            this.statType = s.statType;
            this.modifierType = s.modifierType;
            this.value = s.value;
            this.duration = s.duration;
            this.freezeDuration = s.freezeDuration;
        }
    }
    public enum StatType { MAXHP, ATTACK, DEFENSE }
    public enum ModifierType { FLAT, PERCENTAGE }
}