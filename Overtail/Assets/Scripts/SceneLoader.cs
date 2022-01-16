using System;
using System.Collections;
using System.Collections.Generic;
using Overtail.Items;
using Overtail.Util;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Overtail
{
    public class SceneLoader : MonoBehaviour
    {
        private static SceneLoader _instance;
        public static SceneLoader Instance => _instance;

        public event Action SceneChanged;

        public event Action LoginSceneLoaded;
        public event Action OverWorldSceneLoaded;
        public event Action CombatSceneLoaded;

        public event Action LoginSceneUnloaded;
        public event Action OverWorldSceneUnloaded;
        public event Action CombatSceneUnloaded;


        void Awake()
        {
            MonoBehaviourExtension.MakeSingleton(this, ref _instance, keepAlive: true, destroyOnSceneZero: true);
            SceneManager.sceneLoaded += TriggerLoaded;
            SceneManager.sceneUnloaded += TriggerUnloaded;
        }

        void Start()
        {


        }

        private void TriggerLoaded(Scene next, LoadSceneMode mode)
        {
            Debug.Log($"<color=green>[SceneLoader] Loaded '{next.name}'</color>");
            SceneChanged?.Invoke();
            switch (next.name)
            {
                case "LoginScene":
                    LoginSceneLoaded?.Invoke();
                    break;
                case "OverWorldScene":
                    OverWorldSceneLoaded?.Invoke();
                    break;
                case "CombatScene":
                    CombatSceneLoaded?.Invoke();
                    break;
            }
        }

        private void TriggerUnloaded(Scene prev)
        {
            Debug.Log("<color=orange>[SceneLoader] Unloaded '" + prev.name + "'</color>");
            switch (prev.name)
            {
                case "LoginScene":
                    LoginSceneUnloaded?.Invoke();
                    break;
                case "OverWorldScene":
                    OverWorldSceneUnloaded?.Invoke();
                    break;
                case "CombatScene":
                    CombatSceneUnloaded?.Invoke();
                    break;
            }
        }

        public void LoadCombatScene()
        {
            SceneManager.LoadScene("CombatScene");
        }

        public void LoadOverWorldScene()
        {
            SceneManager.LoadScene("OverWorldScene");
        }
    }
}