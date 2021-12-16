using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Overtail.Entity;

namespace Overtail.Battle
{
    /// <summary>
    /// Buffs and other possible modifiers to characters.
    /// Duration in turns.
    /// </summary>
    [System.Serializable]
    public class StatusEffect
    {
        [SerializeField] private StatType statType;
        [SerializeField] private ModifierType modifierType;
        [SerializeField] private float value;
        [SerializeField] private int duration;
        [SerializeField] private bool freezeDuration;

        public StatType StatType {
            get => statType;
            private set { }
        }
        public ModifierType ModifierType {
            get => modifierType;
            private set { }
        }
        public int Duration {
            get => duration;
            set => duration = Mathf.Max(0, value);
        }
        public float Value {
            get => value;
            private set { }
        }
        public bool FreezeDuration {
            get => freezeDuration;
            private set { }
        }
        public void SetFreeze(bool b) {
            freezeDuration = b;
        }
    }
    public enum StatType { MAXHP, ATTACK, DEFENSE }
    public enum ModifierType { FLAT, PERCENTAGE }
}