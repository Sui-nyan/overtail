using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;
using Overtail.Items;
using Overtail.Collections;
using System;
using Overtail.Items.Components;
using JetBrains.Annotations;

class ItemEditor : EditorWindow
{
    private static string _directory = "./Assets/Resources/Items";

    private static string[] _options = { };
    private static string _filename;
    private static Texture2D _itemIcon;

    private InterfaceDictionary<IItemComponent, bool> _componentToggles { get; } =
        new InterfaceDictionary<IItemComponent, bool>();

    public string _id = "";
    public string _name = "";
    public string _description = "";
    public string _spriteId = "";

    private bool _useCustomFilename = false;

    private bool _equipment;
    private bool _potion;
    private bool _stack;
    private bool _trash;

    private EquipComponent _equipComponent;
    private PotionComponent _potionComponent;
    private StackComponent _stackComponent;
    private TrashComponent _trashComponent;


    // Add menu item named "My Window" to the Window menu
    [MenuItem("OVERTAIL TOOLS/Item Editor")]
    public static void ShowWindow()
    {
        //Show existing window instance. If one doesn't exist, make one.
        EditorWindow.GetWindow(typeof(ItemEditor));
    }

    void OnGUI()
    {
        EditorGUILayout.BeginHorizontal();

        SidebarGUI();
        ItemEditorGui();

        EditorGUILayout.EndHorizontal();
    }

    private void SidebarGUI()
    {
        EditorGUILayout.BeginVertical("box", GUILayout.MaxWidth(150), GUILayout.ExpandHeight(true));

        SelectDirGUI();
        _options = GetFilesFromPath();

        foreach (string filepath in _options) // List files
        {
            if (GUILayout.Button(Path.GetFileName(filepath)))
            {
                LoadFile(filepath);
            }
        }

        EditorGUILayout.EndVertical();
    }


    private void SelectDirGUI()
    {
        EditorGUILayout.BeginHorizontal();

        GUILayout.Label(
            System.IO.Directory.Exists(_directory)
                ? $".../{System.IO.Directory.GetParent(_directory).Name}/{Path.GetFileName(_directory)}"
                : "<Invalid Directory>");

        if (GUILayout.Button("..."))
        {
            string newInput = EditorUtility.OpenFolderPanel("Open Item Folder", "./", "Items");
            if (newInput != "") _directory = newInput;
        }

        EditorGUILayout.EndHorizontal();
    }

    private string[] GetFilesFromPath()
    {
        if (string.IsNullOrEmpty(_directory)) return new string[] { };

        return Directory.GetFiles(_directory).Where(file => file.EndsWith(".json")).ToArray();
    }


    private void ItemEditorGui()
    {
        EditorGUILayout.BeginVertical();

        EditorGUILayout.BeginHorizontal();
        {
            var _s = new GUIStyle(GUI.skin.GetStyle("Label"));
            Color c = _s.normal.textColor;
            c.a = 0.2f;
            _s.normal.textColor = c;

            GUILayout.Label(ShortenString(GetFilePath() ?? "INVALID PATH"), _s);
        }

        if (GUILayout.Button("Save", GUILayout.ExpandWidth(false)))
            SaveFile(GetFilePath());

        if (GUILayout.Button("Clear", GUILayout.ExpandWidth(false)))
            ClearItem();

        EditorGUILayout.EndHorizontal();

        _useCustomFilename = EditorGUILayout.BeginToggleGroup("Custom name", _useCustomFilename);
        _filename = EditorGUILayout.TextField("File name", _filename);
        EditorGUILayout.EndToggleGroup();

        if (!_useCustomFilename) _filename = _id.Replace("overtail:", "") + ".json";

        EditorGUILayout.Space();

        DisplayItemInfo();

        EditorGUILayout.EndVertical();
    }

    private string GetFilePath()
    {
        if (String.IsNullOrEmpty(_filename)) return null;

        var filepath = Path.Combine(Path.GetFullPath(_directory), _filename);
        return filepath.Replace("\\", "/");
    }

    private string ShortenString(string str, int maxLength = 30)
    {
        int startIndex = System.Math.Max(str.Length - maxLength, 0);
        int length = str.Length - startIndex;
        return (str.Length > maxLength ? "..." : "") + str.Substring(startIndex, length);
    }

    private void DisplayItemInfo()
    {
        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.BeginVertical(GUILayout.ExpandWidth(true));
        _id = EditorGUILayout.TextField("ID", _id);
        _name = EditorGUILayout.TextField("Name", _name);
        _description = EditorGUILayout.TextField("Description", _description, GUILayout.Height(80));
        _spriteId = EditorGUILayout.TextField("SpriteFile", _spriteId);

        string imgPath = Path.Combine(_directory, _spriteId).Replace("\\", "/").Replace("./", "");
        {
            var _s = new GUIStyle(GUI.skin.GetStyle("Label"));
            Color c = _s.normal.textColor;
            c.a = 0.2f;
            _s.normal.textColor = c;
            _s.alignment = TextAnchor.MiddleRight;
            EditorGUILayout.LabelField(imgPath, _s);
        }
        EditorGUILayout.EndVertical();

        if (_itemIcon != null) GUILayout.Label(_itemIcon);

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Components", EditorStyles.boldLabel);

        DisplayComponents();
    }

    private void DisplayComponents()
    {
        // Equip Property
        _equipment = EditorGUILayout.BeginToggleGroup("Equipment", _equipment);
        _equipComponent ??= new EquipComponent();
        _equipComponent.MaxHP = EditorGUILayout.IntField("Max HP", _equipComponent.MaxHP);
        _equipComponent.Attack = EditorGUILayout.IntField("Attack", _equipComponent.Attack);
        _equipComponent.Defense = EditorGUILayout.IntField("Defense", _equipComponent.Defense);
        _equipComponent.Charm = EditorGUILayout.IntField("Charm", _equipComponent.Charm);
        EditorGUILayout.EndToggleGroup();

        // Potion property
        _potion = EditorGUILayout.BeginToggleGroup("Potion", _potion);
        _potionComponent ??= new PotionComponent();
        _potionComponent.IsConsumed = EditorGUILayout.Toggle("Consumed on Use", _potionComponent.IsConsumed);
        _potionComponent.HpRecovery = EditorGUILayout.IntField("Recovery Amount", _potionComponent.HpRecovery);
        EditorGUILayout.EndToggleGroup();

        // Stackable Property
        _stack = EditorGUILayout.BeginToggleGroup("Stackable", _stack);
        _stackComponent ??= new StackComponent();
        _stackComponent.MaxQuantity = EditorGUILayout.IntSlider("Max Stack Size", _stackComponent.MaxQuantity, 1, 99);
        EditorGUILayout.EndToggleGroup();

        // Trashable Property
        _trash = EditorGUILayout.BeginToggleGroup("Trashable", _trash);
        _trashComponent ??= new TrashComponent();
        EditorGUILayout.EndToggleGroup();
    }

    private void ClearItem()
    {
        _id = "";
        _name = "";
        _description = "";
        _spriteId = "";

        _useCustomFilename = false;

        _equipment = false;
        _potion = false;
        _stack = false;
        _trash = false;

        _equipComponent = null;
        _potionComponent = null;
        _stackComponent = null;
        _trashComponent = null;
    }

    private void LoadFile(string filepath)
    {
        Debug.Log($"<color=green>Loading item from {filepath}</color>");

        _filename = Path.GetFileName(filepath);

        var jsonItem = JsonUtility.FromJson<Item>(File.ReadAllText(filepath));

        _id = jsonItem.Id;
        _name = jsonItem.Name;
        _description = jsonItem.Description;
        _spriteId = jsonItem.SpriteId;

        _equipComponent = jsonItem.GetComponent<EquipComponent>();
        _potionComponent = jsonItem.GetComponent<PotionComponent>();
        _stackComponent = jsonItem.GetComponent<StackComponent>();
        _trashComponent = jsonItem.GetComponent<TrashComponent>();

        _potion = _potionComponent != null;
        _stack = _stackComponent != null;
        _equipment = _equipComponent != null;
        _trash = _trashComponent != null;

        Debug.Log(
            $"<color=green>{(_equipment ? "Equip " : "X")}{(_potion ? "Potion " : "X")}{(_stack ? "Stack " : "X")}{(_trash ? "Trash" : "X")}</color>");
    }

    private void SaveFile([NotNull] string filepath)
    {
        void Remove<T>() where T : IItemComponent =>
            Debug.Log(0); //new Item().Components.RemoveAll(c => c.GetType() == typeof(T));

        List<IItemComponent> components = new List<IItemComponent>();

        if (_equipment) components.Add(_equipComponent);
        if (_potion) components.Add(_potionComponent);
        if (_stack) components.Add(_stackComponent);
        if (_trash) components.Add(_trashComponent);

        var item = new Item(_id, _name, components, _spriteId, _description);
        File.WriteAllText(filepath, JsonUtility.ToJson(item));
        Debug.Log($"<color=green>Saved item to {filepath}</color");
    }
}
