using System;
using Overtail.Battle.Entity;
using Overtail.Battle;
using UnityEngine;
namespace Overtail.Items
{
    public class PotionSystem : IItemSystem
    {
        public bool IsCompatible(ItemStack itemStack)
        {
            return itemStack.Item.GetComponent<PotionComponent>() != null;
        }

        public void UseItem(ItemStack itemStack, IItemInteractor target)
        {
            PotionComponent u = itemStack.Item.GetComponent<PotionComponent>();
            if (u == null) throw new ArgumentException();

            if (u.IsConsumed) {
                itemStack.Quantity -= 1;
            }

            ApplyEffects(itemStack.Item, target);

            UnityEngine.Debug.Log($"{target} used {itemStack.Item}");
        }

        public bool? IsConsumed(ItemStack itemStack)
        {
            return itemStack.Item.GetComponent<PotionComponent>()?.IsConsumed;
        }

        private void ApplyEffects(Item item, IItemInteractor target)
        {
            PotionComponent c = item.GetComponent<PotionComponent>();

            target.Heal(c.HpRecovery);
            foreach (StatusEffect s in c.effects)
            {
                target.AddStatus(s);
            }
        }
    }
}