using System.Collections.Generic;
using Overtail.Battle.Entity;

namespace Overtail.Items.Components
{
    /// <summary>
    /// Can be used in overworld/general
    /// What does item do (in overworld? or general) when "used"
    /// </summary>
    [System.Serializable]
    public class PotionComponent : IItemComponent
    {
        public bool IsConsumed;
        public List<StatusEffect> effects;
        public int HpRecovery;

        public PotionComponent()
        {
            IsConsumed = true;
            this.effects = new List<StatusEffect>();
            HpRecovery = 0;
        }

        public PotionComponent(PotionComponent potion)
        {
            IsConsumed = potion.IsConsumed;
            this.effects = new List<StatusEffect>(potion.effects);
            HpRecovery = potion.HpRecovery;
        }
    }
}