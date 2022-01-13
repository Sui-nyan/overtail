using Overtail.Battle;
namespace Overtail.Items
{
    /// <summary>
    /// Can be equipped
    /// Equipment which adds to player stats.
    /// </summary>
    [System.Serializable]
    public class EquipComponent : IItemComponent
    {
        public int MaxHP;
        public int Attack;
        public int Defense;
        public int Charm;

        public EquipComponent()
        {
            MaxHP = 0;
            Attack = 0;
            Defense = 0;
            Charm = 0;
        }

        public EquipComponent(int maxHp, int attack, int defense, int charm)
        {
            MaxHP = maxHp;
            Attack = attack;
            Defense = defense;
            Charm = charm;
        }
    }
}