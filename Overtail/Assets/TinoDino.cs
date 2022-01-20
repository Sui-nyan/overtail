using Overtail.Battle.Entity;
using System.Collections;
using System.Collections.Generic;
using Overtail.Battle;
using Overtail.Items;
using UnityEditorInternal;
using UnityEngine;

public class TinoDino : EnemyEntity
{
    private bool firstTurn = true;
    private bool escape = false;
    public override IEnumerator OnGreeting(BattleSystem system)
    {
        yield return system.GUI.StartDialogue("Hey, who are you?!");
        yield return system.GUI.StartDialogue($"Wait! Your name is ... {system.Player.Name} ... I can smell that!");

    }

    public override IEnumerator DoTurnLogic(BattleSystem system)
    {
        if (system.Player.HP >= HP * 2)
        {
            StartCoroutine(OnEscape(system));
            StartCoroutine(system.Player.OnOpponentEscapes(system));
            system.Exit();
            yield break;
        }

        yield return system.GUI.StartDialogue($"Let me finish you");

        if (!firstTurn) yield break;
        firstTurn = false;
        yield return system.GUI.StartDialogue($"*{this.Name.ToUpper()} uses confusion*");
        yield return system.GUI.StartDialogue($"{system.Player.Name} is confused. {system.Player.Name} attacks itself");

        system.Player.HP = (int)(system.Player.HP * 0.9f);
        yield return StartCoroutine(OnAttack(system));
    }


    public override IEnumerator OnAttack(BattleSystem system)
    {
        system.Player.HP -= Mathf.Max(Attack - system.Player.Defense, 0);
        yield break;
    }

    public override IEnumerator OnGetAttacked(BattleSystem system)
    {
        return base.OnGetAttacked(system);
    }


    public override IEnumerator OnGetFlirted(BattleSystem system)
    {
        yield return system.GUI.StartDialogue($"You Disgust me {system.Player.Name}");
        yield return system.GUI.StartDialogue($"You try to get me {this.Name.ToUpper()} to get in your LOVE BAN?!");
        yield return system.GUI.StartDialogue($"OMAE WA MOU SHINDEIRU");
        yield return StartCoroutine(OnGetAttacked(system));

    }

    public override IEnumerator OnBully(BattleSystem system)
    {

        yield return system.GUI.StartDialogue("After I finish you, I finish your mom!");
        yield return StartCoroutine(OnGetAttacked(system));
    }

    public override IEnumerator OnGetBullied(BattleSystem system)
    {
        yield return system.GUI.StartDialogue("Did my breakfast say something?");
        yield return StartCoroutine(OnGetAttacked(system));
    }

    public override IEnumerator OnItemUse(BattleSystem system, ItemStack itemStack)
    {
        return base.OnItemUse(system, itemStack);
    }

    public override IEnumerator OnEscape(BattleSystem system)
    {
        yield return system.GUI.StartDialogue($"See you later Alligator!");
    }

    public override IEnumerator OnOpponentEscapes(BattleSystem system)
    {
        return base.OnOpponentEscapes(system);
    }

}
