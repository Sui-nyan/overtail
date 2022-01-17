using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Overtail.Items.Systems;
using JetBrains.Annotations;
using Overtail.Battle.Entity;
using Overtail.PlayerModule;
using UnityEngine;
using Overtail.GUI;
using Overtail.Util;

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

            _inventory = LoadInvFromAPI();
            _playerObject.Inventory = _inventory;
        }

        private static ItemContainer Placeholder()
        {
            ItemContainer inv = new ItemContainer();

            // Add items here as placeholder
            inv.Append(new ItemStack(ItemDatabase.GetFromId(itemId:"overtail:cat_ears"), quantity: 1));

            return inv;
        }

        private static ItemContainer LoadInvFromAPI() // might not be instantaneous
        {
            UnityEngine.Debug.Log("[InventoryManager] LoadFromAPI()");
            ItemContainer inv = new ItemContainer();

            try
            {
                // TODO: Remove setting API.Token here
                API.API.Token =
                    "TVdWa1pUQmxOekF0TlRoaE1pMDBabVkyTFdGa1pHSXRaR00xTWpJMFkySXpaVFZoLkpESjVKREV3SkV0UlIzQlNRVlV6UWtocWVFVjVRVzFPWnpOSGFTNTZXVzlhTDIxeGJEbDZOekJuV1RsamFtSXpSQzR2V0RCdVVYWnVkR3RMLk1qQXlNaTB3TWkweE5RPT0=";
                string jsonStr = Task.Run(() => API.API.GET("inv")).Result;
                Debug.Log("[InventoryManager] jsonStr written");
                // UnityEngine.Debug.Log("[InventoryManager] " + jsonStr);
                // Get Items from API

                Dictionary<string, string>[] items =
                    JsonConvert.DeserializeObject<Dictionary<string, string>[]>(jsonStr);
                
                foreach (Dictionary<string, string> item in items)
                {
                    // TODO ItemStack == null from API
                    inv.Append(item["id"] == ""
                        ? null
                        : new ItemStack(ItemDatabase.GetFromId(item["id"]), int.Parse(item["amount"])));
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
                Debug.LogError("<color=red>Could not load Inventory from API</color>");
                Debug.LogError(e);
            }

            return inv;
        }

        private bool SaveInvToAPI()
        {
            try
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

                _ = Task.Run(() => API.API.PUT("inv/save", data)); // Save to API
                return true;
            }
            catch (Exception)
            {
                return false; // TODO: Display user friendly error (failed to save)
            }
        }

        public List<ItemStack> GetItems()
        {
            return _inventory.ItemList;
        }

        internal bool IsUseable(ItemStack itemStack)
        {
            return _potionSystem.IsCompatible(itemStack);
        }

        internal bool IsEquippable(ItemStack itemStack)
        {
            return _equipSystem.IsCompatible(itemStack);
        }

        public bool IsTrashable(ItemStack itemStack)
        {
            return _trashSystem.IsCompatible(itemStack);
        }

        internal void UseItem(ItemStack itemStack)
        {
            _potionSystem.UseItem(itemStack, _player);
            PruneEmpty(itemStack);
        }

        internal void UseItem(ItemStack itemStack, IItemInteractor player)
        {
            _potionSystem.UseItem(itemStack, player);
            PruneEmpty(itemStack);
        }

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

        internal void TrashItem(ItemStack itemStack, int quantity = 1)
        {
            _trashSystem.TrashItem(itemStack, quantity); // Reduce from stack
            PruneEmpty(itemStack);
        }

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