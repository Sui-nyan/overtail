using System;
using System.Collections.Generic;
using UnityEngine;


namespace Overtail.Entity
{
    /// <summary>
    /// Base class to define Monsters, NPCs (and main character)
    /// Base stats to define their strengths and dynamically scale with with levels depending on progress
    /// </summary>
    [System.Serializable]
    public class EntityTemplate // : ScriptableObject if other stats, like love/affection
    {
        [SerializeField] private Sprite sprite;
        [SerializeField] private string name;
        public Dictionary<string, string> responses = new Dictionary<string, string>(); // possible implementation
        internal Sprite Sprite { get => sprite; }
        internal string Name { get => name; }
    }
}