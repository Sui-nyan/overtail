using System.Collections;
using System;
using UnityEngine;

namespace Assets.Scripts
{
    public class TestEvents : MonoBehaviour
    {
        UnitCallsEvent1 version1;
        UnitCallsEvent2 version2;

        private void Start()
        {
            version1 = new UnitCallsEvent1();
            version2 = new UnitCallsEvent2();

            version1.GotSomethingToSay += MyResponse1;
            version2.GotSomethingToSay += MyResponse2;
        }

        private void MyResponse2(MessageArgs m)
        {
            Debug.Log($"{m.actor.Name} said \"{m.text}\" to {(m.target != null ? m.target.Name : "no one")}");
        }

        private void MyResponse1(BattleUnit sender, string text, EventArgs e)
        {
            Debug.Log($"{sender.Name} said \"{text}\" to no one");
        }


        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1)) version1.Trigger();
            if (Input.GetKeyDown(KeyCode.Alpha2)) version2.Trigger();
        }
    }

    public class BattleUnit
    {
        private string name;
        public string Name => name;

        public BattleUnit(string name)
        {
            this.name = name;
        }

    }

    public class UnitCallsEvent1
    {
        public delegate void Message(BattleUnit sender, string text, MessageArgs e = null);
        public event Message GotSomethingToSay;

        public void Trigger()
        {
            GotSomethingToSay?.Invoke(new BattleUnit("Peter1"), "Hello");
        }
    }

    public class UnitCallsEvent2
    {
        public delegate void Message(MessageArgs messageObj);
        public event Message GotSomethingToSay;

        public void Trigger()
        {
            GotSomethingToSay?.Invoke(new MessageArgs(new BattleUnit("Peter2"), "Hi there", new BattleUnit("The Goblin")))  ;
        }
    }
    public class MessageArgs : EventArgs
    {
        public readonly BattleUnit actor;
        public readonly BattleUnit target;
        public readonly string text;

        public MessageArgs(BattleUnit actor, string text, BattleUnit target)
        {
            this.actor = actor;
            this.target = target;
            this.text = text;
        }
        public MessageArgs(BattleUnit actor, string text)
        {
            this.actor = actor;
            this.text = text;
        }
    } 
}