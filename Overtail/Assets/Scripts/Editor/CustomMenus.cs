using Overtail.Items;
using UnityEditor;
using UnityEngine;

namespace Overtail.MenuItems
{
    public class CustomMenus : ScriptableObject
    {
        [MenuItem("GameObject/OVERTAIL/Loot (Ground)")]
        static void CreateLootGameObject()
        {
            CreateFromResources<Lootable>("Prefabs/Loot");
        }

        [MenuItem("GameObject/OVERTAIL/Teleporter")]
        static void CreateTeleporter()
        {
            CreateFromResources<Teleporter>("Prefabs/Teleporter");
        }

        private static GameObject CreateFromResources<T>(string path) where T:MonoBehaviour
        {
            var prefab = Resources.Load<T>(path);
            if(prefab == null) Debug.LogError("No prefab found :: " + path);
            GameObject obj = PrefabUtility.InstantiatePrefab(prefab) as GameObject;

            Selection.activeObject = obj;
            Selection.activeGameObject = obj;
            return obj;
        }
    }
}