using UnityEngine;
namespace Overtail.Battle
{
    public class PlayerUnit : BattleUnit
    {
        public Overtail.Player.PlayerSerializable data;

        public void OpenInventory()
        {
            Debug.Log(new Items.Item(name = "Apple", true) == new Items.Item(name = "Apple", true));
            if (data.Inventory.Contains(new Items.Item(name="Apple",true))) Debug.Log("Gotit");
        }
    }

    
}
