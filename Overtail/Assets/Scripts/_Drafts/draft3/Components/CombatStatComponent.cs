using UnityEngine;
namespace Draft3.Unityless
{
    public class CombatStatComponent : Component
    {
        private int maxhp, atk, def, hp, level;
        public int MaxHP => maxhp;
        public int Attack => atk;
        public int Defense => def;
        public int HP => hp;
        public int Level => level;
    }
}