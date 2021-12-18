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
        [SerializeField] private GameObject attackButton;
        [SerializeField] private GameObject interactButton;
        [SerializeField] private GameObject inventoryButton;
        [SerializeField] private GameObject escapeButton;

        private BattleSystem system;
        private BattleUnit player;
        private BattleUnit enemy;
        private GameObject[] Buttons => new GameObject[] {
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

            foreach (GameObject b in Buttons)
            {
                b.GetComponent<Button>().onClick.AddListener(HideButtons);
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
            foreach (GameObject b in Buttons)
            {
                if (b == null) throw new System.Exception("Button is null - Buttons might not have been assigned in Unity");
                b.SetActive(true);
            }
        }

        public void HideButtons()
        {
            foreach (GameObject b in Buttons)
            {
                b.SetActive(false);
            }
        }
    }
}