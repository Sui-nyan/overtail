using System;
using UnityEditor;
using UnityEngine;

namespace Overtail.Items
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class Lootable : MonoBehaviour
    {
        [SerializeField] public ItemStack stack;


        private SpriteRenderer spriteRenderer;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();

            gameObject.AddComponent<CircleCollider2D>();
        }

        void Start()
        {
            // Refresh
            stack.Item = ItemDatabase.GetFromId(stack.Item.Id);
            spriteRenderer.sprite = stack.Item.Sprite;
        }

        public void Interact()
        {
            if (InventoryManager.Instance?.PickUp(stack) ?? false)
            {
                Destroy(this.gameObject);
            }
        }


        public static Lootable Instantiate(Item item, int quantity, Vector3 pos)
        {
            var go = new GameObject();
            go.name = item.Name + "x" + quantity;
            if (pos != null) go.transform.position = pos;

            var loot = go.AddComponent<Lootable>();

            var stack = new ItemStack();
            stack.Item = item;
            stack.Quantity = quantity;

            loot.stack = stack;
            loot.spriteRenderer.sprite = loot.stack.Item.Sprite;

            return loot;
        }
    }
}
