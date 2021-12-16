using System.Collections.Generic;
using UnityEngine;

namespace Overtail.Items
{
    [System.Serializable]
    public class Inventory
    {
        [SerializeField] private List<ItemListing> items;
        [SerializeField] private int capacity;
        [SerializeField] private bool infiniteCapacity;

        public extern bool AddItem(Item item);

        public bool Contains(Item item)
        {
            //Debug.Log($"??? {item}");
            foreach (ItemListing listing in items)
            {
                //Debug.Log($"Checking {listing.Item} : {listing.Item.Equals(item)}");
                if (listing.Item.Equals(item)) return true;
            }
            return false;
        }

        public bool Contains(string itemName)
        {
            foreach (ItemListing listing in items)
            {
                if (listing.Item.Name == itemName) return true;
            }
            return false;
        }
        public bool UseItem(Item item)
        {
            return false;
        }
        public bool UseItem(string itemName)
        {
            return false;
        }


        public extern int FindItem(Item item);
        public extern Item FindItem(int index);
        public extern Item PopAt(int index);
        public extern Item GetAt(int index);
        public extern void RemoveAll();
        public extern void Prune();

    }
}