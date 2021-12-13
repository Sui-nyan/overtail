using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overtail.Battle
{
    /// <summary>
    /// Class managing HUDs in battle.
    /// Attach to parent HUD object.
    /// </summary>
    public class BattleHUD : MonoBehaviour
    {
        public Text nameText;
        public Text levelText;
        public Slider hpSlider;

        public void SetHUD(BattleUnit unit)
        {
            nameText.text = unit.Name;
            levelText.text = "Lvl " + unit.Level;
            hpSlider.maxValue = unit.HP;
            hpSlider.value = unit.CurrentHP;
        }

        public void SetHP(int hp)
        {
            hpSlider.value = hp;
        }
    }
}