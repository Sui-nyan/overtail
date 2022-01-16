using Overtail.Battle.Entity;
using UnityEngine;
using Overtail;
using Overtail.Battle;
using Overtail.Items;
using Overtail.Map;
using Overtail.Util;

namespace Overtail.PlayerModule
{
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

        public bool IsFreeRoaming { get; private set; }

        // Initialization


        // Inventory

        [SerializeReference] public ItemContainer Inventory;

        // Item interaction

        [SerializeReference] private Item _main;
        [SerializeReference] private Item _off;
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


        private bool __enabled = true;

        void OnMouseDown()
        {
            var t = FindObjectOfType<WaterTilemap>();
            if (t is null) return;

            if (__enabled) t.DisableWater();
            else t.EnableWater();

            __enabled ^= true;
        }
    }
}