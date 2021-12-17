using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Overtail.Battle
{
    [System.Serializable]
    public class BattleGUI
    {
        [Header("UI Elements")]
        [SerializeField] private BattleHUD playerHUD;
        [SerializeField] private BattleHUD enemyHUD;
        [SerializeField] private Text textBox;

        [Header("Buttons")]
        [SerializeField] private UnityEngine.GameObject attackButton;
        [SerializeField] private UnityEngine.GameObject interactButton;
        [SerializeField] private UnityEngine.GameObject inventoryButton;
        [SerializeField] private UnityEngine.GameObject escapeButton;

        private BattleSystem system;
        private BattleUnit player;
        private BattleUnit enemy;
        private UnityEngine.GameObject[] Buttons => new UnityEngine.GameObject[] {
            attackButton,
            interactButton,
            inventoryButton,
            escapeButton
        };

        public void Setup(BattleSystem system)
        {
            this.system = system;
            this.player = this.system.Player;
            this.enemy = this.system.Enemy;

            foreach (UnityEngine.GameObject b in Buttons)
            {
                b.GetComponent<Button>().onClick.AddListener(this.HideButtons);
            }
        }
        public void UpdateHUD()
        {
            playerHUD.SetHUD(player);
            enemyHUD.SetHUD(enemy);
        }

        public void SetText(string text)
        {
            textBox.text = text;
        }

        public void ReselectedGUI()
        {
            if (EventSystem.current.currentSelectedGameObject == null)
            {
                EventSystem.current.SetSelectedGameObject(attackButton);
            }
        }

        public void ShowButtons()
        {
            foreach (UnityEngine.GameObject b in Buttons)
            {
                if (b == null) throw new System.Exception("Button is null - Buttons might not have been assigned in Unity");
                b.SetActive(true);
            }
        }

        public void HideButtons()
        {
            foreach (UnityEngine.GameObject b in Buttons)
            {
                b.SetActive(false);
            }
        }
    }
}