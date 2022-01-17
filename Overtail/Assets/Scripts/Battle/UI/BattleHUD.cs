using System.Collections;
using Overtail.Battle.Entity;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overtail.Battle.UI
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
        public void UpdateHUD(BattleEntity entity)
        {
            if (_taskRunning) return;
            StartCoroutine(C_UpdateHUD(entity));
        }

        private IEnumerator C_UpdateHUD(BattleEntity entity)
        {
            _taskRunning = true;
            yield return StartCoroutine(SmoothValues(entity, 2));
            _taskRunning = false;
        }

        private IEnumerator SmoothValues(BattleEntity entity, float time)
        {
            // Debug.LogWarning(entity.Name + "::" + "SmoothValues");
            float timeElapsed = 0f;

            var prevName = _name;
            var prevLevel = _level;
            var prevMaxHp = _maxHp == 0 ? entity.MaxHP : _maxHp;
            var prevHp = _hp;

            while (true)
            {
                timeElapsed += Time.deltaTime;
                yield return null;

                _name = entity.Name;
                _level = (int)Mathf.SmoothStep(prevLevel, entity.Level, timeElapsed / time);
                _maxHp = Mathf.SmoothStep(prevMaxHp, entity.MaxHP, timeElapsed / time);
                _hp = Mathf.SmoothStep(prevHp, entity.HP, timeElapsed / time);

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

        public IEnumerator SmoothExp(float progress)
        {
            // TODO
            yield break;
        }
    }
}
