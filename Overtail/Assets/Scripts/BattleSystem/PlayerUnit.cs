﻿using Overtail.Pending;
using System;
using System.Collections;
using UnityEngine;
namespace Overtail.Battle
{
    public class PlayerUnit : BattleUnit
    {
        public int Exp { get; set; }
        public int Charm { get; private set; }

        public void Load(PlayerData data)
        {
            // TODO
        }
        public void Save(PlayerData data)
        {
            // TODO
        }

        public override IEnumerator OnAttack(BattleSystem system)
        {
            yield break;
        }

        public override IEnumerator OnFlirt(BattleSystem system)
        {
            // Placeholder
            yield return system.GUI.StartDialogue("You found some corny lyrics");
            yield return system.GUI.StartDialogue("Can you hear me?");
            yield return system.GUI.StartDialogue("Im talking to you");
            yield return system.GUI.StartDialogue("Across the water");
            yield return system.GUI.StartDialogue("Across the deep, blue, ocean");
            yield return system.GUI.StartDialogue("Under the open sky");
            yield return system.GUI.StartDialogue("Oh my, baby im tryin.");

            yield break;
        }

        public override IEnumerator OnBully(BattleSystem system)
        {
            yield return system.GUI.StartDialogue($"{Name} is trying to sh*t-talk {system.Enemy.Name}");
            yield return new WaitUntil(() => system.IsIdle);
            yield return new WaitForSeconds(1f);
        }
        public override IEnumerator OnItemUse(BattleSystem system, ItemStack itemStack)
        { 
            yield break;
        }



        public override IEnumerator OnVictory(BattleSystem system)
        {
            p.Exp += 0; // system.Enemy.EXPValue

            // CheckForLevelUp();

            yield break;
        }
    }


}
