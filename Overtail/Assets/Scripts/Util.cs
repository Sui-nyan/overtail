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

            // Base Scaling from Pokemon games :sparkles:
            int f(int b)
            {
                return (int)(b * 2 * level / 100f) + 5;
            }
            total.Attack = f(baseStats.Attack);
            total.Defense = f(baseStats.Defense);
            total.MaxHealth = f(baseStats.MaxHealth) + 5 + level;

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
                multiplier.Add(t,0);
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

            total.Attack = (int)(total.Attack * (1 + multiplier[StatType.Attack])) + flat[StatType.Attack];
            total.Defense = (int)(total.Defense * (1 + multiplier[StatType.Defense])) + flat[StatType.Defense];
            total.MaxHealth = (int)(total.MaxHealth * (1 + multiplier[StatType.MaxHealth])) + flat[StatType.MaxHealth];

            return total;
        }

        private static Stats Sum(Stats a, Stats b)
        {
            Stats sum = new Stats();

            sum.Attack = a.Attack + b.Attack;
            sum.Defense = a.Defense + b.Defense;
            sum.MaxHealth = a.MaxHealth + b.MaxHealth;
            sum.Health = a.Health + b.Health;

            return sum;
        }
    }
}