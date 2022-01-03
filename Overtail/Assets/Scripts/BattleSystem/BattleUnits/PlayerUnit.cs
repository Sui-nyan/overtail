using System.Collections;
using UnityEngine;
namespace Overtail.Battle
{
    public class PlayerUnit : BattleUnit
    {
        [SerializeField] private PersistentPlayerData persistentData;
        [Header("Debug")]
        [SerializeField] private PlayerSerializable p;

        private void Start()
        {
            Load();
        }

        public void End()
        {
            Save();
        }

        private void Load()
        {
            p = persistentData.playerSerializable;

            name = p.Name;
            level = p.Level;
            hp = p.HP;
            maxHp = p.MaxHP;
            attack = p.Attack;
            defense = p.Defense;

            statusEffects.Clear();
            p.StatusEffects.ForEach(s => statusEffects.Add(new StatusEffect(s)));
        }
        private void Save()
        {
            p.Exp = 0;// EXP++

            p.Level = level;
            p.HP = hp;
            p.MaxHP = maxHp;
            p.Attack = attack;
            p.Defense = defense;

            p.StatusEffects.Clear();
            statusEffects.ForEach(s => p.StatusEffects.Add(new StatusEffect(s)));
        }

        protected override int GetStat(StatType statType)
        {
            return base.GetStat(statType);
        }

        public IEnumerator Magic()
        {
            SpeakLine("Hello");
            SpeakLine("Im sad");
            SpeakLine("Please dont hurt me");

            yield break;
        }
    }


}
