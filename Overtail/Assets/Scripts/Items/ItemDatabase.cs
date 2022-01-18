using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;

// Loaded once on game start. Library of ALL available items in game.
namespace Overtail.Items
{
    public static class ItemDatabase
    {
        private static readonly List<Item> _items = new List<Item>();

        internal static List<Item> GetAll()
        {
            // TODO deep copy
            return _items;
        }

        public static List<string> GetAllIds()
        {
            return _items.Select(i => i.Id).ToList();
        }


        static ItemDatabase()
        {
            LoadItems(); // TODO
        }

        public static void LoadItems()
        {
            _items.Clear();

            UnityEngine.Debug.Log("[ItemDatabase] Load()");
            LoadJsonFromResources("Items");
        }

        private static void LoadJsonFromResources(string subFolder = "Items")
        {
            var jsonFiles = Resources.LoadAll<TextAsset>(subFolder);

            foreach (var file in jsonFiles)
            {
                var i = JsonUtility.FromJson<Item>(file.text);

                // TODO unify
                var id = Regex.Replace(i.SpriteId, @"\..*", "");
                i.Sprite = Resources.Load<Sprite>($"{subFolder}/{id}"); // Sprite

                _items.Add(i);
            }
        }


        private static void LoadJsonFromPath(string dir = @"./Assets/Resources/Items")
        {
            // All files with '.json' extension
            string[] allFiles = Directory.GetFiles(dir).Where(file => file.EndsWith(".json")).ToArray();

            foreach (var file in allFiles)
            {
                var absPath = Path.GetFullPath(file);
                var i = JsonUtility.FromJson<Item>(File.ReadAllText(absPath));
                _items.Add(i);
            }
        }

        public static Item GetFromId(string itemId)
        {
            var i = _items.Find(i => i.Id == itemId);
            if (i == null) UnityEngine.Debug.LogWarning("Unknown item id:" + itemId);
            return i;
        }

        public static Item GetFromIndex(int index)
        {
            return _items[index];
        }
    }
}
