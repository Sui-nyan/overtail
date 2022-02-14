using Overtail.Battle.Entity;
using Overtail.Dialogue;
using Overtail.GUI;
using UnityEngine;
using Overtail.Items;
using Overtail.Map;
using Overtail.Util;
using UnityEngine.SceneManagement;

namespace Overtail.PlayerModule
{
    /// <summary>
    /// Core Player class, accessed from many other classes.
    /// </summary>
    [RequireComponent(typeof(PlayerMovement))]
    [RequireComponent(typeof(StatComponent))]
    [DisallowMultipleComponent]
    public class Player : MonoBehaviour, IItemInteractor
    {
        private static Player _instance;

        void Awake()
        {
            MonoBehaviourExtension.MakeSingleton(this, ref _instance, keepAlive: true, destroyOnSceneZero: true);
        }

        // Fields

        public string Name { get; }

        // Flags/States

        public bool IsFreeRoaming{
            get
            {
                bool inMenu = FindObjectOfType<MenuManager>()?.MenuIsActive ?? false;
                bool inDialogue = FindObjectOfType<DialogueManager>()?.IsOpen ?? false;
                bool inCombat = SceneManager.GetActiveScene().name.Contains("Combat");
                return !(inMenu || inDialogue || inCombat);
            } 
        } 

        // Initialization


        // Inventory

        [SerializeField] public ItemContainer Inventory;

        // Item interaction

        [SerializeField] private Item _main;
        [SerializeField] private Item _off;

        public Item MainHand
        {
            get => _main;
            set => _main = value;
        }
        public Item OffHand { get => _off; set => _off = value; }

        public void Heal(int hp)
        {
            var stats = GetComponent<StatComponent>();
            stats.HP = Mathf.Clamp(stats.HP, 1, stats.MaxHP);

        }

        public void AddStatus(StatusEffect newEffect)
        {
            throw new System.NotImplementedException();
        }
    }
}
