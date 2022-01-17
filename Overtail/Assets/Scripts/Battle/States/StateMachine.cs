using UnityEngine;

namespace Overtail.Battle.States
{
    public abstract class StateMachine : MonoBehaviour
    {
        protected State _state;

        public void SetState(State state)
        {
            if (this._state != null)
            {
                StartCoroutine(this._state.CleanUp());
            }

            this._state = state;
            StartCoroutine(this._state.Start());
        }

        public void RestartState()
        {
            StartCoroutine(_state.Start());
        }
    }
}
