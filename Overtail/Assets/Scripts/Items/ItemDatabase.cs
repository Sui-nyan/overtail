using System.Collections.Generic;
using System;
using System.Linq;
using System.IO;
using UnityEngine;

// Loaded once on game start. Library of ALL available items in game.
namespace Overtail.Items
{
    public static class ItemDatabase
    {
        private static readonly List<Item> _items = new List<Item>();

        public static List<Item> GetAll()
        {
            Debug.LogWarning("DEV ENVIRONMENT ONLY");
            return _items;
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

                var _ = Resources.Load<Sprite>($"{subFolder}/{file.name}"); // Sprite

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
    }
}