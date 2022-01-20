using System;
using Overtail.Dialogue;
using Overtail.PlayerModule;
using UnityEditor;
using UnityEngine;

namespace Overtail.Items
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class Lootable : MonoBehaviour, IInteractable
    {
        [SerializeField] public ItemStack stack;

        private SpriteRenderer spriteRenderer;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            gameObject.AddComponent<CircleCollider2D>();
            var rb = gameObject.AddComponent<Rigidbody2D>();
            rb.gravityScale = 0;
            rb.drag = 5;
            rb.angularDrag = .5f;
        }

        void Start()
        {
            // Refresh
            stack.Item = ItemDatabase.GetFromId(stack.Item.Id);
            spriteRenderer.sprite = stack.Item.Sprite;
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

            loot.spriteRenderer.sortingLayerID =
                FindObjectOfType<Player>().GetComponent<SpriteRenderer>().sortingLayerID;
            loot.gameObject.AddComponent<PositionalRenderSorter>();

            return loot;
        }

        public void Interact(Interactor interactor)
        {
            if (InventoryManager.Instance?.PickUp(stack) ?? false)
            {
                Destroy(this.gameObject);
            }
        }
    }
}
