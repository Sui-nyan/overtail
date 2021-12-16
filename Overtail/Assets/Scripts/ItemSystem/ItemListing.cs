using UnityEngine;

namespace Overtail.Items
{

    [System.Serializable]
    class ItemListing
    {
        [SerializeField] private int quantity;
        [SerializeField] private Item item;

        public Item Item => item;
        public int Quantity
        {
            get => quantity;
            set => quantity = value; // TODO Redundant?
        }
    }

}