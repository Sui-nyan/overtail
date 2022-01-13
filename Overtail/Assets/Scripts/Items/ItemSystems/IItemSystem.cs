namespace Overtail.Items
{
    /// <summary>
    /// Different ways to interact with items!<para/>
    /// 
    /// Defines how data from components is handled #Behaviour
    /// <para/>
    /// Example in menu
    /// <code>
    /// if(UseItem.IsAvailable(myShinySword)) CreateUseButton();
    /// Button.AddListener(() => UseItem.OnUse(myShinySword))
    /// </code>
    /// </summary>
    public interface IItemSystem
    {
        public bool IsCompatible(ItemStack itemStack);
    }
}
