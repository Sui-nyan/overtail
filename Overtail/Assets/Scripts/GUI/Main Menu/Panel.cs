using System.Globalization;
using System.Text;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Overtail.GUI
{
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