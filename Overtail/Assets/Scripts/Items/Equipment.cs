using UnityEngine;
using Overtail.Entity;

namespace Overtail.Items
{
    /// <summary>
    /// Equpment definition. Probably inherits from `class Item` later.
    /// </summary>
    [CreateAssetMenu(fileName = "equipment_generic", menuName = "Create Equipment/Generic")]
    public class Equipment : ScriptableObject
    {
        private readonly new string name;
        public string Name => name;

        [TextArea]
        private readonly string description;
        public string Description => description;

        private readonly int attack;
        public int Attack => attack;

        [SerializeField]
        private readonly int defense;
        public int Defense => defense;
    }
}