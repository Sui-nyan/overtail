using System.Collections;
using System.Data;
using TMPro;
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
        public TextMeshProUGUI nameText;
        public TextMeshProUGUI levelText;
        public Slider hpSlider;
        public TextMeshProUGUI hpText;

        private string _name;
        private int _level = 0;
        private float _hp = 0;
        private float _maxHp = 0;

        public float HpSliderSmoothTime = 1.5f;

        public void UpdateHUD(BattleUnit unit)
        {
            StartCoroutine(SmoothValues(unit, 2));
        }

        private IEnumerator SmoothValues(BattleUnit unit, float time)
        {
            float timeElapsed = 0f;

            var prevName = _name;
            var prevLevel = _level;
            var prevMaxHp = _maxHp == 0 ? unit.MaxHP : _maxHp;
            var prevHp = _hp;

            while (true)
            {
                timeElapsed += Time.deltaTime;
                yield return null;

                _name = unit.Name;
                _level = (int)Mathf.SmoothStep(prevLevel, unit.Level, timeElapsed / time);
                _maxHp = Mathf.SmoothStep(prevMaxHp, unit.MaxHP, timeElapsed / time);
                _hp = Mathf.SmoothStep(prevHp, unit.HP, timeElapsed / time);

                ApplyGui();

                if (timeElapsed > time) break;
            }

            yield break;
        }

        private void ApplyGui()
        {
            nameText.text = _name;
            levelText.text = "Lvl " + _level;
            hpSlider.maxValue = _maxHp;
            hpSlider.value = _hp;
            hpText.text = (int)_hp + "/" + (int)_maxHp;
        }
    }
}