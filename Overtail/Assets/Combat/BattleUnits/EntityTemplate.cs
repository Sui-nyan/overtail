using System;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class EntityTemplate // : ScriptableObject if other stats, like love/affection
{
    [SerializeField] private Sprite sprite;
    [SerializeField] private string name;
    [SerializeField] private Stats baseStats;
    public Dictionary<string, string> responses = new Dictionary<string, string>(); // possible implementation
    internal Sprite Sprite { get => sprite; }
    internal string Name { get => name; }
    internal Stats BaseStats { get => baseStats; }
}





