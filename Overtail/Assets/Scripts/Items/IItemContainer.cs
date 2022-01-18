using System.Collections.Generic;
using System;

namespace Overtail.Items
{
    // Inventory as an item stash (could also be a storage chest)
    internal interface IItemContainer
    {
        List<ItemStack> ItemList { get; }

        bool AddItems(ItemStack itemStack);
        bool AddItems(Item item, int quantity = 1);
        bool RemoveItems(Item item, int quantity = 1);
        bool RemoveItems(ItemStack stack, int quantity = 1);

        bool IsFull();
        bool Append(ItemStack itemStack);
        void Clear();
        bool Contains(ItemStack itemStack);
        bool Exists(Predicate<ItemStack> match);
#nullable enable
        ItemStack? Find(Predicate<ItemStack> match);
        ItemStack? FindLast(Predicate<ItemStack> match);
#nullable disable
        List<ItemStack> FindAll(Predicate<ItemStack> match);
        void ForEach(Action<ItemStack> action);
        List<ItemStack>.Enumerator GetEnumerator();
        void Insert(int index, ItemStack itemStack);
        bool Remove(ItemStack itemStack);
        int RemoveAll(Predicate<ItemStack> match);
        void Sort();
        ItemStack[] ToArray();
    }
}
