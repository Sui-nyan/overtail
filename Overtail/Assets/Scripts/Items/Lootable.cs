using System;
using Overtail.Dialogue;
using Overtail.PlayerModule;
using UnityEditor;
using UnityEngine;

namespace Overtail.Items
{
    /// <summary>
    /// Lootable items on the ground.
    /// </summary>
    [RequireComponent(typeof(SpriteRenderer))]
    public class Lootable : MonoBehaviour, IInteractable
    {
        [SerializeField] public ItemStack stack;

        private SpriteRenderer spriteRenderer;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            var col = gameObject.AddComponent<CircleCollider2D>();
            col.radius = 0.25f;
            var rb = gameObject.AddComponent<Rigidbody2D>();
            rb.gravityScale = 0;
            rb.drag = 5;
            rb.angularDrag = 2f;

        }

        void Start()
        {
            // Refresh
            stack.Item = ItemDatabase.GetFromId(stack.Item.Id);
            spriteRenderer.sprite = stack.Item.Sprite;
        }

        
        /// <summary>
        /// Instantiates a ground loot at a specified position
        /// </summary>
        /// <param name="item"></param>
        /// <param name="quantity"></param>
        /// <param name="pos"></param>
        /// <returns></returns>
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
            if (!FindObjectOfType<Player>().IsFreeRoaming) return;
            if (InventoryManager.Instance?.PickUp(stack) ?? false)
            {
                Destroy(this.gameObject);
            }
        }
    }
}
