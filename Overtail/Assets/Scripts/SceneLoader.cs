using System;
using System.Collections;
using System.Collections.Generic;
using Overtail.Items;
using Overtail.PlayerModule;
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

        void Update()
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                if (Input.GetKeyDown(KeyCode.Alpha0)) SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                if (Input.GetKeyDown(KeyCode.Alpha1)) LoadOverWorldScene();
                if (Input.GetKeyDown(KeyCode.Alpha2)) LoadCombatScene();
            }
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
                    FindObjectOfType<Player>().GetComponent<SpriteRenderer>().enabled = true;
                    OverWorldSceneLoaded?.Invoke();
                    break;
                case "CombatScene":
                    FindObjectOfType<Player>().GetComponent<SpriteRenderer>().enabled = false;
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

        public static void LoadCombatScene()
        {
            SceneManager.LoadScene("CombatScene");
        }

        public static void LoadOverWorldScene()
        {
            SceneManager.LoadScene("OverWorldScene");
        }
    }
}