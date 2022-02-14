using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Overtail.Util
{
    public static class MonoBehaviourExtension
    {
        /// <summary>
        /// Singleton Shorthand
        /// Add this to Awake() of any monobehaviour to make it singleton
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="staticInstance"></param>
        /// <param name="keepAlive"></param>
        /// <param name="destroyOnSceneZero"></param>
        public static void MakeSingleton<T>(T obj, ref T staticInstance, bool keepAlive = true, bool destroyOnSceneZero = true) where T : MonoBehaviour
        {
            if (staticInstance != null && staticInstance != obj)
            {
                Debug.LogWarning($"[{obj.transform.parent?.name ?? obj.name}] Singleton {obj.GetType().Name} already exists. [{staticInstance.transform.parent?.name ?? staticInstance.name}].");
                Object.Destroy(obj.gameObject);
            }
            else
            {
                staticInstance = obj;
            }

            if (keepAlive && null == obj.transform.parent) Object.DontDestroyOnLoad(obj);
            if (destroyOnSceneZero) obj.StartCoroutine(LateAwake());

            IEnumerator LateAwake()
            {
                yield return null;
                SceneManager.sceneLoaded += Dispose;
            }

            void Dispose(Scene scene, LoadSceneMode mode)
            {
                if (scene.buildIndex != 0) return;
                SceneManager.sceneLoaded -= Dispose;
                Debug.LogWarning("DISPOSE" + obj.GetType().Name);
                Object.Destroy(obj.gameObject);
            }
        }
    }
}
