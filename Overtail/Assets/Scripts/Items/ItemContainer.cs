using System.Collections.Generic;
using System;
using System.Linq;
using Overtail.Items.Components;
using UnityEngine;

namespace Overtail.Items
{
    [System.Serializable]
    public class ItemContainer
    {
        [SerializeField]
        private List<ItemStack> _itemList = new List<ItemStack>();
        public List<ItemStack> ItemList => _itemList;


        [SerializeField]
        private int _capacity = 16;
        public int Capacity => _capacity;


        public bool AddItems(ItemStack itemStack)
        {
            return AddItems(itemStack.Item, itemStack.Quantity);
        }

        public bool AddItems(Item item, int quantity = 1)
        {
            if (item == null) throw new ArgumentNullException("Null Item");

            var quantityMax = Math.Max((item.GetComponent<StackComponent>()?.MaxQuantity ?? 0), 1);

            var emptySlots = _itemList.Count(s => s == null) + Math.Max(_capacity - _itemList.Count, 0);
            var spaceToTopUp = _itemList
                .Where(s => s?.Item?.Equals(item) ?? false)
                .Sum(s => Math.Max(quantityMax - s.Quantity, 0));

            var spaceAvailable = spaceToTopUp + emptySlots * quantityMax;
            if (spaceAvailable < quantity) return false; // Not enough space

            for (var i = 0; i < _itemList.Count; i++)
            {
                if (quantity == 0) return true;

                var stack = _itemList[i];

                if (stack == null) // Empty Slot
                {
                    _itemList[i] = new ItemStack();
                    _itemList[i].Item = item;
                    _itemList[i].Quantity = Math.Min(quantityMax, quantity);

                    quantity -= _itemList[i].Quantity;
                }
                else if (stack?.Item.Equals(item) ?? false) // If same item
                {
                    if (stack.Quantity >= quantityMax) continue; // Already fully stacked

                    var quantityAdded = Math.Min(quantity, (quantityMax - stack.Quantity)); // How much can fit in

                    stack.Quantity += quantityAdded;
                    quantity -= quantityAdded;
                }
            }

            while (_capacity > _itemList.Count && quantity > 0) // Append to end
            {
                var quantityAdded = Math.Min(quantity, quantityMax);
                quantity -= quantityAdded;
                Append(new ItemStack(item, quantityAdded));
            }

            if (quantity != 0) throw new InvalidOperationException($"Method concluded with unexpected quantity={quantity}");
            return true;
        }

        public bool RemoveItems(ItemStack itemStack, int quantity = 1)
        {
            if (itemStack.Quantity < quantity) return false;

            itemStack.Quantity -= quantity;

            var index = FindIndex(s => s == itemStack);
            if (index >= 0) _itemList[index] = null;

            return true;
        }

        public bool Append(ItemStack itemStack)
        {
            if (_itemList.Count < _capacity)
            {
                _itemList.Add(itemStack);
                return true;
            }

            return false;
        }

        public bool IsFull()
        {
            var emptySlot = _itemList.Exists(s => s == null);
            var capacityReached = _itemList.Count >= _capacity;
            return !emptySlot && capacityReached;
        }

        public bool RemoveItems(Item item, int quantity = 1)
        {
            var quantityAvailable = _itemList.Where(i => i.Item?.Equals(item) ?? false).Sum(i => i.Quantity);

            if (quantityAvailable < quantity) return false;

            for (var i = 0; i < _itemList.Count; i++)
            {
                var stack = _itemList[i];

                if (!(stack?.Item?.Equals(item)) ?? true) continue; // Empty slot/nullStack or nullItem

                var quantityRemoved = Math.Min(stack.Quantity, quantity);
                stack.Quantity -= quantityRemoved;
                quantity -= quantityRemoved;

                if (stack.Quantity == 0) _itemList[i] = null; // Slot is now empty

                if (quantity == 0) return true;
            }

            throw new InvalidOperationException($"Method concluded with invalid quantity={quantity}");
        }

        public void Sort()
        {
            SortAlpha();
        }

        public void SortAlpha()
        {
            static int AlphaComparison(ItemStack a, ItemStack b)
            {
                var nameOrder = String.Compare(a.Item.Name, b.Item.Name);

                if (nameOrder != 0)
                    return nameOrder;
                else
                    return a.Quantity - b.Quantity;
            }

            Sort(AlphaComparison);
        }

        public void SortNumeric()
        {
            Sort((a, b) => a.Quantity - b.Quantity);
        }

        #region Generic Implementation
        public void Add(ItemStack itemStack)
        {
            _itemList.Add(itemStack);
        }
        public void Clear()
        {
            _itemList.Clear();
        }
        public bool Contains(ItemStack itemStack)
        {
            return _itemList.Contains(itemStack);
        }
        public bool Exists(Predicate<ItemStack> match)
        {
            return _itemList.Exists(match);
        }
        public ItemStack Find(Predicate<ItemStack> match)
        {
            return _itemList.Find(match);
        }
        public List<ItemStack> FindAll(Predicate<ItemStack> match)
        {
            return _itemList.FindAll(match);
        }
        public ItemStack FindLast(Predicate<ItemStack> match)
        {
            return _itemList.FindLast(match);
        }
        public void ForEach(Action<ItemStack> action)
        {
            _itemList.ForEach(action);
        }
        public List<ItemStack>.Enumerator GetEnumerator()
        {
            return _itemList.GetEnumerator();
        }
        public void Insert(int index, ItemStack itemStack)
        {
            _itemList.Insert(index, itemStack);
        }
        public bool Remove(ItemStack itemStack)
        {
            return _itemList.Remove(itemStack);
        }
        public int RemoveAll(Predicate<ItemStack> match)
        {
            return _itemList.RemoveAll(match);
        }
        private int FindIndex(Predicate<ItemStack> match)
        {
            return _itemList.FindIndex(match);
        }
        public void Sort(Comparison<ItemStack> c)
        {
            _itemList.Sort(c);
        }
        public ItemStack[] ToArray()
        {
            return _itemList.ToArray();
        }
        #endregion
    }
}
