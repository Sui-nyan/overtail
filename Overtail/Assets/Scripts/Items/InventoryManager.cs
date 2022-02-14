using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Newtonsoft.Json;
using UnityEngine;
using Overtail.PlayerModule;
using Overtail.Util;
using Overtail.Items.Systems;

namespace Overtail.Items
{
    public class InventoryManager : MonoBehaviour
    {
        private static InventoryManager _instance;
        public static InventoryManager Instance => _instance;

        private Player _playerObject;

        private IItemInteractor _player;
        [SerializeField] private ItemContainer _inventory;
        private PotionSystem _potionSystem;
        private EquipSystem _equipSystem;
        private TrashSystem _trashSystem;

        void Awake()
        {
            MonoBehaviourExtension.MakeSingleton(this, ref _instance, keepAlive: true, destroyOnSceneZero: true);

            _playerObject = FindObjectOfType<Player>();
            _player = _playerObject;
            _potionSystem = new PotionSystem();
            _equipSystem = new EquipSystem();
            _trashSystem = new TrashSystem();

            _inventory = LoadAPIPlayerData();
            _playerObject.Inventory = _inventory;
        }

        private static ItemContainer Placeholder()
        {
            ItemContainer inv = new ItemContainer();

            // Add items here as placeholder
            inv.Append(new ItemStack(ItemDatabase.GetFromId(itemId: "overtail:cat_ears"), quantity: 1));

            return inv;
        }


        private struct APIPosition
        {
            public float x;
            public float y;
            public string scene;
        }


        private static ItemContainer LoadAPIPlayerData() // might not be instantaneous
        {
            Debug.Log("[InventoryManager] LoadFromAPI()");
            ItemContainer inv = new ItemContainer();

            try
            {
                // Get Items from API
                string jsonStr = Task.Run(() => API.GET("playerData")).Result;
                Debug.Log("[InventoryManager] jsonStr written");
                // UnityEngine.Debug.Log("[InventoryManager] " + jsonStr);

                var playerData = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonStr);
                /* var pos = JsonConvert.DeserializeObject<APIPosition>(playerData["pos"]);

                // Player position
                var playerPos = new Vector2(pos.x, pos.y);
                Debug.Log(playerPos);
                Player player = FindObjectOfType<Player>();
                Rigidbody2D rb = player.gameObject.GetComponent<Rigidbody2D>();
                rb.MovePosition(playerPos);   // Move player to the saved position */

                // Inventory
                Debug.Log(playerData["inv"]);
                var invArr = JsonConvert.DeserializeObject<Dictionary<string, string>[]>(playerData["inv"]);

                foreach (Dictionary<string, string> item in invArr)
                {
                    inv.Append(new ItemStack(ItemDatabase.GetFromId(item["id"]), int.Parse(item["amount"])));
                }
            }
            catch (Exception e)
            {
                Debug.LogError("[API] <color=red>Could not load Inventory from API</color>");
                Debug.LogError("[API]" + e);
            }

            return inv;
        }

        /// <summary>
        /// Saves inventory content to database
        /// </summary>
        public void SaveInvToAPI()
        {
            // JSON encode inventory data
            string data = "[";

            foreach (ItemStack stack in _inventory.ItemList)
            {
                data += "{";
                data += "\"id\": \"" + stack?.Item.Id + "\"";
                data += "\"amount\": \"" + stack?.Quantity + "\"";
                data += "}";
            }

            data += "]";

            Task.Run(() => API.POST("inv/save", new Dictionary<string, string> { { "invData", data } })); // Save to API
        }

        /// <summary>
        /// Returns the list of items in inventory
        /// </summary>
        /// <returns></returns>
        public List<ItemStack> GetItems()
        {
            return _inventory.ItemList;
        }

        /// <summary>
        /// Tells whether an item is usable
        /// </summary>
        /// <param name="itemStack"></param>
        /// <returns></returns>
        internal bool IsUseable(ItemStack itemStack)
        {
            return _potionSystem.IsCompatible(itemStack);
        }

        /// <summary>
        /// Returns whether item is equippable
        /// </summary>
        /// <param name="itemStack"></param>
        /// <returns></returns>
        internal bool IsEquippable(ItemStack itemStack)
        {
            return _equipSystem.IsCompatible(itemStack);
        }

        /// <summary>
        /// returns whether item can be trashed
        /// </summary>
        /// <param name="itemStack"></param>
        /// <returns></returns>
        public bool IsTrashable(ItemStack itemStack)
        {
            return _trashSystem.IsCompatible(itemStack);
        }


        internal void UseItem(ItemStack itemStack)
        {
            _potionSystem.UseItem(itemStack, _player);
            PruneEmpty(itemStack);
        }

        /// <summary>
        /// Uses item. Contains logic about how data from potion component should be handled
        /// </summary>
        /// <param name="itemStack"></param>
        internal void UseItem(ItemStack itemStack, IItemInteractor player)
        {
            _potionSystem.UseItem(itemStack, player);
            PruneEmpty(itemStack);
        }

        /// <summary>
        /// Equips item. Contains logic about how data from equip component should be handled
        /// </summary>
        /// <param name="itemStack"></param>
        internal void EquipItem(ItemStack itemStack)
        {
            // 0) Equippable? (Yes, unless UI fcks up)

            if (_player.MainHand != null) // 1) Player has something equipped already?
            {
                if (!_inventory.IsFull()) // 2) Inventory has space?
                {
                    _inventory.AddItems(_player.MainHand); // 3) Unequip current
                    _player.MainHand = null;
                }
                else
                {
                    throw new NotImplementedException("Unavailable action.");
                }
            }

            _equipSystem.EquipItem(itemStack, _player);
            PruneEmpty(itemStack);
        }

        /// <summary>
        /// Trashes item. Contains logic about how data from trash component should be handled
        /// </summary>
        /// <param name="itemStack"></param>
        internal void TrashItem(ItemStack itemStack, int quantity = 1)
        {
            _trashSystem.TrashItem(itemStack, quantity); // Reduce from stack
            PruneEmpty(itemStack);
        }

        /// <summary>
        /// Tries to pick up an item. Returns false if not available (e.g. inventory is full)
        /// </summary>
        /// <param name="itemStack"></param>
        /// <returns></returns>
        public bool PickUp(ItemStack itemStack)
        {
            if (!_inventory.IsFull())
            {
                _inventory.AddItems(itemStack);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Button to sort inventory
        /// </summary>
        internal void OnSortInventory()
        {
            _inventory.SortAlpha();
            PruneEmpty();
        }

        /// <summary>
        /// Check if stack is empty, prune from inventory
        /// </summary>
        /// <param name="itemStack"></param>
        private void PruneEmpty([CanBeNull] ItemStack itemStack)
        {
            if (itemStack?.Quantity == 0)
            {
                _inventory.Remove(itemStack);
            }
        }

        private void PruneEmpty()
        {
            _inventory.RemoveAll(stack => stack == null || stack?.Quantity == 0);
        }

        private void Dispose()
        {
            SceneLoader.Instance.LoginSceneLoaded -= Dispose;
            Destroy(this.gameObject);
        }
    }
}
