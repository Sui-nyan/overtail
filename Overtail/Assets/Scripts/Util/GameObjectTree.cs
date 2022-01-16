using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.Animations;

namespace Overtail.Util
{
    public static class GameObjectTree
    {
        public static T[] GetAllChildren<T>(GameObject parent) where T : MonoBehaviour
        {
            List<T> match = new List<T>();

            foreach (Transform child in parent.transform)
            {
                if (child.TryGetComponent<T>(out var component)) match.Add(component);
                match.AddRange(GetAllChildren<T>(child.gameObject));
            }

            return match.ToArray();
        }

        // Overloads
        public static T[] GetAllChildren<T, P>(P parent)
            where T : MonoBehaviour where P : MonoBehaviour =>
            GetAllChildren<T>(parent.gameObject);

        public static T[] GetAllChildren<T>(Transform parent)
            where T : MonoBehaviour =>
            GetAllChildren<T>(parent.gameObject);

        public static GameObject[] GetChildrenWithComponent<T>(GameObject parent) where T : MonoBehaviour
        {
            return GetAllChildren<T>(parent).Select(obj => obj.gameObject).ToArray();
        }

        public static GameObject[] GetChildrenWithComponent<T, P>(P parent)
            where T : MonoBehaviour where P : MonoBehaviour
            => GetChildrenWithComponent<T>(parent.gameObject);

        public static GameObject[] GetChildrenWithComponent<T>(Transform parent)
            where T : MonoBehaviour
            => GetChildrenWithComponent<T>(parent.gameObject);

        public static GameObject[] GetChildrenWithTag(string tag, GameObject parent)
        {
            List<GameObject> match = new List<GameObject>();

            foreach (Transform child in parent.transform)
            {
                if (child.gameObject.tag.Equals(tag)) match.Add(child.gameObject);
            }

            return match.ToArray();
        }


        public static T FindSingleObjectOfType<T>(GameObject root = null) where T : MonoBehaviour
        {
            var pgs = GameObject.FindObjectsOfType<T>();
            if (pgs.Length > 1)
            {
                foreach (var item in pgs)
                {
                    UnityEngine.Debug.LogWarning(item);
                }

                throw new InvalidOperationException(
                    $"[{root?.GetType().Name}] Multiple ({pgs.Length}){pgs.GetType().GetElementType()} found");
            }

            return pgs.Length > 0 ? pgs[0] : null;
        }
    }
}