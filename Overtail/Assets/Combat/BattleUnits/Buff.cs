using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Buffs and other possible modifiers to characters.
/// Duration in turns.
/// </summary>
[System.Serializable]
public class Buff
{
    public StatType Type;
    public bool Multiplicative;
    public float Value;

    [SerializeField] private int duration;
    public int Duration { get => duration; set => System.Math.Max(0, value); }
    public bool Permanent;
}

public enum StatType { ATK, DEF, HP }
