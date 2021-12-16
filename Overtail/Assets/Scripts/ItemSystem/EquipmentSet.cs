using System;
using UnityEngine;

namespace Overtail.Items
{
    /// <summary>
    /// Encapsulated whole equipment set for more compact method calls
    /// </summary>

    [System.Serializable]
    public class EquipmentSet
    {
        public Weapon weapon = null;
        public HeadEquipment head = null;
        public BodyEquipment body = null;

        public Equipment[] all { get => new Equipment[] { head, body, weapon }; }
    }

}