using System.Collections;
using UnityEngine;
using Overtail.Entity;
using Overtail.Items;
using Overtail.Battle;
using System.Collections.Generic;

namespace Overtail
{
    public static class Util
    {
        public static Stats CalculateStats(int level, Stats baseStats, EquipmentSet equipment, List<Buff> buffs)
        {
            Stats total = new Stats();

            // Base Scaling
            total.ATK = baseStats.ATK + level;
            total.DEF = baseStats.DEF + level;
            total.HP = baseStats.HP + level * 10;

            // + Equipment
            foreach (Equipment e in equipment.all)
            {
                if (e != null) total = Sum(total, e.Stats);
            }

            // + Buffs
            Dictionary<StatType, float> multiplier = new Dictionary<StatType, float>();
            Dictionary<StatType, int> flat = new Dictionary<StatType, int>();

            foreach (StatType t in System.Enum.GetValues(typeof(StatType)))
            {
                multiplier.Add(t, 1);
                flat.Add(t, 0);
            }

            // % buffs
            foreach (Buff buff in buffs.FindAll(b => b.Multiplicative))
            {
                multiplier[buff.Type] += buff.Value;
            }

            // + buffs
            foreach (Buff buff in buffs.FindAll(b => !b.Multiplicative))
            {
                flat[buff.Type] += (int)buff.Value;
            }

            total.ATK = (int)(total.ATK * (1 + multiplier[StatType.ATK])) + flat[StatType.ATK];
            total.DEF = (int)(total.DEF * (1 + multiplier[StatType.DEF])) + flat[StatType.DEF];
            total.HP = (int)(total.HP * (1 + multiplier[StatType.HP])) + flat[StatType.HP];

            return total;
        }

        private static Stats Sum(Stats a, Stats b)
        {
            Stats sum = new Stats();

            sum.ATK = a.ATK + b.ATK;
            sum.DEF = a.DEF + b.DEF;
            sum.HP = a.HP + b.HP;
            sum.currentHP = a.currentHP + b.currentHP;

            return sum;
        }
    }
}