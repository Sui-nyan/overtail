using System.Collections;
using UnityEngine;

namespace Overtail.Pending
{
    public abstract class Item
    {
        public abstract string Name { get; }
    }

    public abstract class ItemStack
    {
        public Item Item { get; }
        public int Quantity { get; }
    }
    public abstract class InventoryManager
    {
        public static InventoryManager Instance { get => null; }

        public void UseItem(ItemStack itemStack) { }
    }
}