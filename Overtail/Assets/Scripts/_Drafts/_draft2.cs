using System.Collections;
using UnityEngine;
using System;

namespace Brainstorm.Draft2.WithCommands
{

    public class PlayerAI : IBattleBehaviour
    {
        public ICommand GetAction(BattleUnit actor, BattleUnit opponent)
        {
            // Move through UI and choose action
            return null;
        }
    }

    public class BattleSystem
    {
        BattleUnit Player;
        BattleUnit Enemy = new SlimeAI();

        public enum BattleState { BATTLESETUP, ENEMYTURN, PLAYERTURN, VICTORY, DEFEAT}
        BattleState upcomingState;
        void BattleRoutine()
        {
            upcomingState = BattleState.BATTLESETUP;
            upcomingState = BattleState.PLAYERTURN;
            PlayerTurn();
        }

        void EnemyTurn()
        {
            ICommand action = Enemy.GetAction(Enemy, Player);
            action.Execute(Enemy, Player);

            CheckForGameOver();

            upcomingState = BattleState.PLAYERTURN;
            PlayerTurn();
        }

        void PlayerTurn()
        {
            ICommand action = Player.GetAction(Player, Enemy);
            action.Execute(Player, Enemy);

            CheckForGameOver();

            upcomingState = BattleState.ENEMYTURN;
            EnemyTurn();
        }

        void CheckForGameOver()
        {
            if (upcomingState == BattleState.VICTORY)
                VictoryRoutine();

            if (upcomingState == BattleState.DEFEAT)
                DefeatRoutine();
        }

        void VictoryRoutine()
        {
            // "Congrats", exp, loot and stuff
            CleanUpBattle();
        }
        void DefeatRoutine()
        {
            // "oof" or "try again" or whatever
            CleanUpBattle();
        }

        void CleanUpBattle()
        {
            // Load overworld scene
        }

        void ReactToPlayerDeathEvent()
        {
            upcomingState = BattleState.DEFEAT;
        }

        void ReactToEnemyDeaethEvent()
        {
            upcomingState = BattleState.VICTORY;
        }
    }
    public class BattleUnit : MonoBehaviour, IBattleBehaviour
    {
        public int hp, def, atk;
        public int currentHp;
        public int level;

        public IComponent spriteRenderer;
        public IBattleBehaviour behaviour;

        public virtual ICommand GetAction(BattleUnit actor, BattleUnit opponent)
        {
            throw new System.NotImplementedException();
        }

        void Start() { }


        public void TakeDamage(BattleUnit attacker)
        {
            TakeDamage(attacker, 1);
        }

        public void TakeDamage(BattleUnit attacker, float attackMultiplier)
        {
            TakeDamage(Math.Max(0, (int)(attacker.atk * attackMultiplier) - this.def));
        }

        public void TakeDamage(float percent)
        {
            TakeDamage((int)(percent * this.hp));
        }

        public void TakeDamage(int flat)
        {
            currentHp -= flat;
            CheckAlive();
        }

        void SendEvent_Death()
        {

        }

        void CheckAlive()
        {
            if (currentHp <= 0)
            {
                //dieded
                currentHp = 0;
                SendEvent_Death(); // this should trigger END in battle system;
            }
        }
    }


    #region AIs
    public class SlimeAI : BattleUnit
    {
        public override ICommand GetAction(BattleUnit actor, BattleUnit opponent)
        {
            if (this.currentHp < hp * 0.3)
            {
                return new EvolveAndAttack();
            }
            else return new SpecialAttack();
        }

        private class SpecialAttack : ICommand
        {
            public void Execute(BattleUnit actor, BattleUnit target)
            {

            }
        }

        private class EvolveAndAttack : ICommand
        {
            public void Execute(BattleUnit actor, BattleUnit target)
            {
                GameObject go = actor.gameObject;
                Sprite someSpriteByPath = null;
                go.GetComponent<SpriteRenderer>().sprite = someSpriteByPath;

                SendEventToDialogueBox("Silme EVOLVED");
                // yield return new WaitForSeconds(2);?
            }

            void SendEventToDialogueBox(string text) { }
        }
    }

    #endregion
    #region Generic Actions
    public class BasicAttack : ICommand
    {
        public void Execute(BattleUnit actor, BattleUnit target)
        {
            SendEventToDialogueBox(actor.name + " attacks " + target.name);
            target.TakeDamage(actor);
        }

        void SendEventToDialogueBox(string text) { }
    }
    #endregion

    #region Interfaces
    public interface IBattleBehaviour : IComponent
    {
        ICommand GetAction(BattleUnit actor, BattleUnit opponent);
    }

    public interface ICommand
    {
        void Execute(BattleUnit actor, BattleUnit target);
    }

    #endregion

    #region Pseudo

    public interface IComponent
    {

    }


    #endregion
}