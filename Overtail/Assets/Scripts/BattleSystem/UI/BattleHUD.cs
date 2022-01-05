using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

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
        public Text hpText;

        public float HpSliderSmoothTime = 1.5f;

        public void SetHUD(BattleUnit unit)
        {
            Debug.Log(nameText.text);
            nameText.text = unit.Name;
            levelText.text = "Lvl " + unit.Level;
            hpSlider.maxValue = unit.MaxHP;
            hpSlider.value = unit.HP;
            hpText.text = unit.HP + "/" + unit.MaxHP;
        }

        public IEnumerator SmoothSliderUpdate(BattleUnit unit)
        {
            return SmoothSliderUpdate(unit, HpSliderSmoothTime);
        }

        public IEnumerator SmoothSliderUpdate(BattleUnit unit, float smoothTime)
        {
            var start = hpSlider.value;

            float timeElapsed = 0f;

            Debug.Log($"Smooth HP Slider [{start} -> {unit.HP}]");

            while (hpSlider.value != unit.HP)
            {
                timeElapsed += Time.deltaTime;

                hpSlider.value = Mathf.SmoothStep(start, unit.HP, timeElapsed/smoothTime);
                hpText.text = (int)hpSlider.value + "/" + unit.MaxHP;

                yield return new WaitForEndOfFrame();
            }
        }
    }
}