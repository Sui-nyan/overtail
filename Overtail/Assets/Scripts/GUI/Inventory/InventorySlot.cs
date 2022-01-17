using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Overtail;
using Overtail.GUI;
using UnityEngine;
using UnityEngine.UI;
using Overtail.Items;
using Overtail.Items.Components;
using Overtail.PlayerModule;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using PointerType = UnityEngine.PointerType;

[RequireComponent(typeof(Button))]
public class InventorySlot : MonoBehaviour
{
    [SerializeReference] private ItemStack stack;
    public ItemStack Stack
    {
        get => stack;
        set
        {
            SetupButton();
            stack = value;
        }
    }

    private Text label;
    private Image icon;
    private Button button;

    [SerializeField] private Vector2 offset;
    [SerializeField] private GameObject subMenuButtonPrefab;
    [SerializeField] private GameObject subMenuPrefab;

    private InventoryPanel panel;
    private GameObject root;
    
    void Awake()
    {
        panel = FindObjectOfType<InventoryPanel>();

        label = GetComponentsInChildren<Text>().First(c => c.gameObject != this.gameObject);
        icon = GetComponentsInChildren<Image>().First(c => c.gameObject != this.gameObject);
        button = GetComponent<Button>();

        root = GameObject.FindObjectOfType<InventoryPanel>().gameObject;

        if (subMenuButtonPrefab is null || subMenuPrefab is null) throw new ArgumentNullException();
        if (label is null || icon is null || button is null) throw new ArgumentNullException();
    }

    private void SetupButton()
    {
        if (!SceneManager.GetActiveScene().name.Equals("CombatScene"))
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(CreateSubMenu);
        }
    }
    
    public void Update()
    {
        // Hide/Show Icon & Label
        label.gameObject.SetActive(stack != null);
        icon.gameObject.SetActive(stack != null);

        if (stack != null) SetContent();
    }

    private void SetContent()
    {
        
        icon.sprite = stack.Item?.Sprite;
        label.text = stack.Quantity > 0 ? stack.Quantity.ToString() : "";
    }


    private void CreateSubMenu()
    {
        var usable = stack?.Item.GetComponent<PotionComponent>() != null;
        var equippable = stack?.Item.GetComponent<EquipComponent>() != null;
        var trashable = stack?.Item.GetComponent<TrashComponent>() != null;

        if (!usable && !equippable && !trashable)
        {
            panel.SetSubMenu(null);
            return;
        }

        var subMenu = Instantiate(subMenuPrefab, root.transform);
        subMenu.transform.position = (Vector2) transform.position + offset;
        subMenu.transform.localScale = Vector3.one;
        panel.SetSubMenu(subMenu);

        List<GameObject> buttonObjects = new List<GameObject>();

        if (usable) buttonObjects.Add(CreateOption("Use", OnUse, subMenu.transform));
        if (equippable) buttonObjects.Add(CreateOption("Equip", OnEquip, subMenu.transform));
        if (trashable) buttonObjects.Add(CreateOption("Drop", OnTrash, subMenu.transform));

        var buttons = buttonObjects.Select(obj => obj.GetComponent<Button>()).ToList();

        for (int i = 0; i < buttons.Count; i++)
        {
            var nav = buttons[i].navigation;
            
            nav.mode = Navigation.Mode.Explicit;
            nav.selectOnDown = buttons[(i + 1) % buttons.Count];
            nav.selectOnUp = buttons[(buttons.Count + i - 1) % buttons.Count];

            buttons[i].navigation = nav;
        }

        EventSystem.current.SetSelectedGameObject(buttonObjects[0]);
    }

    private GameObject CreateOption(string label, Action f, Transform parent)
    {
        var obj = Instantiate(subMenuButtonPrefab, parent);
        obj.name = label;
        obj.GetComponentInChildren<Text>().text = label;
        Destroy(obj.GetComponent<ContentSizeFitter>());


        void CancelSubMenu() 
        {
            panel.CloseSubMenu();
            InputManager.Instance.KeyCancel -= CancelSubMenu;
        };
        InputManager.Instance.KeyCancel += CancelSubMenu;

        obj.GetComponent<Button>().onClick.AddListener(() =>
        {
            f();
            panel.CloseSubMenu();
        });

        return obj;
    }

    private void OnUse()
    {
        InventoryManager.Instance.UseItem(stack);
    }

    private void OnEquip()
    {
        Debug.LogError("Equip not implemented yet");
    }

    private void OnTrash()
    {
        Lootable.Instantiate(stack.Item, stack.Quantity, GameObject.FindObjectOfType<Player>().transform.position);
        InventoryManager.Instance.TrashItem(stack, stack.Quantity);
        Debug.Log($"Dropped {stack}");
    }
}