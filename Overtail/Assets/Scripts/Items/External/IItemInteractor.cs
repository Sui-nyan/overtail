using System;
using System.Collections.Generic;
using Overtail.Battle.Entity;
using Overtail.Battle;

namespace Overtail.Items
{
    // player wears weapon/armor
    public interface IItemInteractor
    {
        Item MainHand { get; set; }
        Item OffHand { get; set; }

        void Heal(int hp);

        void AddStatus(StatusEffect newEffect);

        // public Inventory inventory { get; set; }
    }
}