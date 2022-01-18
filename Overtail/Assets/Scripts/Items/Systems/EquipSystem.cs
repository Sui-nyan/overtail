using System;
using Overtail.Items.Components;

namespace Overtail.Items.Systems
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
