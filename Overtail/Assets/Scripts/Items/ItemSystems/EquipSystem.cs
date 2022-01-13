using System.Collections;
using UnityEngine;
using System;

namespace Overtail.Items
{
    /// <summary>
    /// Handles equipping items
    /// </summary>
    public class EquipSystem : IItemSystem
    {
        public bool IsCompatible(ItemStack itemStack)
        {
            return itemStack.Item.GetComponent<EquipComponent>() != null;
        }

        public void EquipItem(ItemStack itemStack, IItemInteractor target)
        {
            if (itemStack.Item.GetComponent<EquipComponent>() == null) throw new ArgumentException();

            target.MainHand = itemStack.Item;
        }
    }
}