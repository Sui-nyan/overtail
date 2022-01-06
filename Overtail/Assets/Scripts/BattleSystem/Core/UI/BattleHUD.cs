using System.Collections;
using System.Data;
using System.Reflection;
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

        private bool _taskRunning = false;
        public void UpdateHUD(BattleUnit unit)
        {
            if (_taskRunning) return;
            StartCoroutine(C_UpdateHUD(unit));
        }

        private IEnumerator C_UpdateHUD(BattleUnit unit)
        {
            _taskRunning = true;
            yield return StartCoroutine(SmoothValues(unit, 2));
            _taskRunning = false;
        }

        private IEnumerator SmoothValues(BattleUnit unit, float time)
        {
            Debug.LogWarning(unit.Name + "::" + "SmoothValues");
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