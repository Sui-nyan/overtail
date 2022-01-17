using System.Collections;
using Overtail.Items;

namespace Overtail.Battle.States
{
    public abstract class State
    {
        protected readonly BattleSystem _system;
        public State(BattleSystem system)
        {
            this._system = system;
        }

        public virtual IEnumerator Start()
        {
            yield break;
        }

        public virtual IEnumerator Attack()
        {
            yield break;
        }

        public virtual IEnumerator Flirt()
        {
            yield break;
        }

        public virtual IEnumerator Bully()
        {
            yield break;
        }

        public virtual IEnumerator UseItem(ItemStack itemStack)
        {
            yield break;
        }

        public virtual IEnumerator Escape()
        {
            yield break;
        }

        public virtual IEnumerator CleanUp()
        {
            yield break;
        }
    }
}
