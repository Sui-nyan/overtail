using UnityEngine;
using Overtail.Entity;

namespace Overtail.Items
{
    /// <summary>
    /// Equpment definition. Probably inherits from `class Item` later.
    /// </summary>
    [CreateAssetMenu(fileName = "equipment_generic", menuName = "Create Equipment/generic")]
    public class Equipment : ScriptableObject
    {
        private new string name;
        public string Name;
        [TextArea]
        public string Description;
        public Stats Stats;
    }
}