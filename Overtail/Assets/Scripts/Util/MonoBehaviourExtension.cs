﻿using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity;
namespace Overtail.Util
{
    public static class MonoBehaviourExtension
    {
        public static void MakeSingleton<T>(T obj, ref T staticInstance, bool keepAlive = true, bool destroyOnSceneZero = true) where T : MonoBehaviour
        {
            if (staticInstance != null && staticInstance != obj)
            {
                Debug.LogWarning($"[{obj.transform.parent?.name ?? obj.name}] Singleton {obj.GetType().Name} already exists. [{staticInstance.transform.parent?.name ?? staticInstance.name}].");
                UnityEngine.Object.Destroy(obj.gameObject);
            }
            else
            {
                staticInstance = obj;
            }

            if (keepAlive && null == obj.transform.parent) UnityEngine.Object.DontDestroyOnLoad(obj);
            if(destroyOnSceneZero) obj.StartCoroutine(LateAwake());

            IEnumerator LateAwake()
            {
                yield return null;
                SceneManager.sceneLoaded += Dispose;
            }

            void Dispose(Scene scene, LoadSceneMode mode)
            {
                if (scene.buildIndex != 0) return;
                SceneManager.sceneLoaded -= Dispose;
                UnityEngine.Debug.LogWarning("DISPOSE" + obj.GetType().Name);
                UnityEngine.Object.Destroy(obj.gameObject);
            }
        }
    }
}