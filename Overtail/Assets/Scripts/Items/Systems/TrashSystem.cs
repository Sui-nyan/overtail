using System;
using Overtail.Items.Components;
using UnityEngine;

namespace Overtail.Items.Systems
{
    public class TrashSystem : IItemSystem
    {
        public bool IsCompatible(ItemStack itemStack)
        {
            return itemStack.Item.GetComponent<TrashComponent>() != null;
        }

        public void TrashItem(ItemStack itemStack, int quantity = 1)
        {
            if (itemStack.Item.GetComponent<TrashComponent>() == null) throw new ArgumentException();

            itemStack.Quantity = Mathf.Max(0, itemStack.Quantity - quantity);
        }
    }
}
