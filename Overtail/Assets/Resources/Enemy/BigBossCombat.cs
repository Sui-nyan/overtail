using System.Collections;
using System.Collections.Generic;
using Overtail.Battle;
using Overtail.Battle.Entity;
using Overtail.Items;
using UnityEngine;

public class BigBossCombat : EnemyEntity
{
    public override IEnumerator OnGreeting(BattleSystem system)
    {

        yield return system.GUI.StartDialogue($"HELLO {system.Player.Name}");
        yield return base.OnGreeting(system);
    }

    public override IEnumerator DoTurnLogic(BattleSystem system)
    {
        if (HP < 0.4 * MaxHP)
        {
            // use potion
            yield return system.GUI.StartDialogue($"{Name} starts to breath heavily");
        }
    }

    public override IEnumerator OnVictory(BattleSystem system)
    {
        return base.OnVictory(system);
    }

    public override IEnumerator OnDefeat(BattleSystem system)
    {
        return base.OnDefeat(system);
    }

    public override IEnumerator OnAttack(BattleSystem system)
    {
        return base.OnAttack(system);
    }

    public override IEnumerator OnGetAttacked(BattleSystem system)
    {
        return base.OnGetAttacked(system);
    }


    public override IEnumerator OnGetFlirted(BattleSystem system)
    {
        return base.OnGetFlirted(system);
    }

    public override IEnumerator OnGetBullied(BattleSystem system)
    {
        return base.OnGetBullied(system);
    }

    public override IEnumerator OnItemUse(BattleSystem system, ItemStack itemStack)
    {
        return base.OnItemUse(system, itemStack);
    }

    public override IEnumerator OnEscape(BattleSystem system)
    {
        return base.OnEscape(system);
    }

    public override IEnumerator OnOpponentEscapes(BattleSystem system)
    {
        return base.OnOpponentEscapes(system);
    }
}
