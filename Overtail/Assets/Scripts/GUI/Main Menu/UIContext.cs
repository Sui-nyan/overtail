using System;
using UnityEngine;

namespace Overtail.GUI
{
    public abstract class UIContext : MonoBehaviour
    {
        private UIContext previous;

        public event Action<UIContext> ContextChanged; //next context

        public UIContext ExitContext()
        {
            Debug.Log($"Exit {this.name} => {previous?.name}");

            this.OnExitUI();
            
            previous?.OnEnterUI();
            ContextChanged?.Invoke(previous);

            return previous;
        }

        public void EnterContext(UIContext nestedContext)
        {
            Debug.Log($"Enter {this.name} => {nestedContext?.name}");
            this.OnExitUI();

            nestedContext.OnEnterUI();
            ContextChanged?.Invoke(nestedContext);

            nestedContext.previous = this;
        }

        public static void EnterRoot(UIContext context)
        {
            context.OnEnterUI();
            context.previous = null;
            
            context.ContextChanged?.Invoke(context);
        }

        protected UIContext GetPreviousContext()
        {
            return previous;
        }

        protected virtual void OnExitUI() { }

        protected virtual void OnEnterUI() { }
    }
}