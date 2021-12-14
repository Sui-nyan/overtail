using System.Collections;
using UnityEngine;

namespace Overtail.Battle
{
    public abstract class State
    {
        protected readonly BattleSystem _system;
        public State(BattleSystem system)
        {
            _system = system;
        }
        public virtual IEnumerator Start()
        {
            yield break;
        }

        public virtual IEnumerator Attack()
        {
            yield break;
        }

        public virtual IEnumerator Interact()
        {
            yield break;
        }

        public virtual IEnumerator Inventory()
        {
            yield break;
        }

        public virtual IEnumerator Escape()
        {
            yield break;
        }

        public virtual IEnumerator Stop()
        {
            yield break;
        }
    }
}