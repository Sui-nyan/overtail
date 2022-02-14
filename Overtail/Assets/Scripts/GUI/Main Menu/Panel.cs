using UnityEngine;

namespace Overtail.GUI
{

    /// <summary>
    /// Submenus are divided into Panels.
    /// </summary>
    [DisallowMultipleComponent]
    public class Panel : MonoBehaviour
    {
        public virtual void Reload()
        {

        }

        public virtual void EnterUI()
        {
            throw new System.NotImplementedException();
        }

        public virtual void ExitUI()
        {
            throw new System.NotImplementedException();
        }
    }
}
