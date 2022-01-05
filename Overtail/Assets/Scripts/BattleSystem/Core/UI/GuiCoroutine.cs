using System;
using System.Collections;
using UnityEngine;

namespace Overtail.Battle
{
    public partial class BattleGUI
    {
        public class GuiCoroutine
        {
            public float PostEventDelay = 0f;
            public Func<MonoBehaviour, IEnumerator> CoroutineHandle; // IEnumerator func(MonoBehaviour obj);
        }
    }
}