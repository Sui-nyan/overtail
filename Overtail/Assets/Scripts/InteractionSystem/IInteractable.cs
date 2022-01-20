using Overtail.PlayerModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overtail.Dialogue
{
    public interface IInteractable
    {
        void Interact(IInteractor player);
    }

    public interface IInteractor
    {

    }
}
