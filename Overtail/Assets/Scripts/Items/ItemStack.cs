namespace Overtail.Items
{
    // Unit of item. "One slot" in inventory
    [System.Serializable]
    public class ItemStack
    {
        public Item Item;
        public int Quantity;

        public ItemStack()
        {

        }

        public ItemStack(Item item, int quantity = 1)
        {
            if (item == null || quantity < 0)
            {
                UnityEngine.Debug.LogWarning($"Created ItemStack with [Item:{(item == null ? "NULL" : item.ToString())} x {quantity}]" +
                                             "\nAre you sure?");
            }

            Item = item;
            Quantity = quantity;
        }

        public override string ToString()
        {
            return $"{Item?.Id ?? "ItemNULL"} [x {Quantity}]";
        }
    }
}
