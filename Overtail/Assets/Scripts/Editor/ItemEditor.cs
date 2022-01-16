using System.Collections;
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

    private Item _item;

    private bool _useCustomFilename = false;

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
        if (_directory != "" && _directory != null)
            return Directory.GetFiles(_directory).Where(file => file.EndsWith(".json")).ToArray();
        else
            return new string[] {null};
    }


    private void ItemEditorGui()
    {
        if (_item == null) SetBlankItem();


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
            SetBlankItem();

        EditorGUILayout.EndHorizontal();

        _useCustomFilename = EditorGUILayout.BeginToggleGroup("Custom name", _useCustomFilename);
        _filename = EditorGUILayout.TextField("File name", _filename);
        EditorGUILayout.EndToggleGroup();

        if (!_useCustomFilename) _filename = _item.Id.Replace("overtail:", "") + ".json";

        EditorGUILayout.Space();

        DisplayItemInfo(_item);

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

    private void DisplayItemInfo(Item item)
    {
        if (item == null) return;

        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.BeginVertical(GUILayout.ExpandWidth(true));
        item.Id = EditorGUILayout.TextField("ID", item.Id);
        item.Name = EditorGUILayout.TextField("Name", item.Name);
        item.Description = EditorGUILayout.TextField("Description", item.Description, GUILayout.Height(80));
        item.SpriteId = EditorGUILayout.TextField("SpriteFile", item.SpriteId);

        string imgPath = Path.Combine(_directory, item.SpriteId).Replace("\\", "/").Replace("./", "");

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

        DisplayComponents(item);
    }
    
    private void DisplayComponents(Item item)
    {
        // Equip Property
        _componentToggles[typeof(EquipComponent)] = BeginToggleGroup<EquipComponent>("Equipment");
        {
            EquipComponent component = GetOrCreateComponent<EquipComponent>(item);

            component.MaxHP = EditorGUILayout.IntField("Max HP", component.MaxHP);
            component.Attack = EditorGUILayout.IntField("Attack", component.Attack);
            component.Defense = EditorGUILayout.IntField("Defense", component.Defense);
            component.Charm = EditorGUILayout.IntField("Charm", component.Charm);
        }
        EditorGUILayout.EndToggleGroup();
        EditorGUILayout.Space();

        // Potion property
        _componentToggles[typeof(PotionComponent)] = BeginToggleGroup<PotionComponent>("Potion");
        {
            PotionComponent component = GetOrCreateComponent<PotionComponent>(item);

            component.IsConsumed = EditorGUILayout.Toggle("Consumed on Use", component.IsConsumed);
            component.HpRecovery = EditorGUILayout.IntField("Recovery Amount", component.HpRecovery);
        }
        EditorGUILayout.EndToggleGroup();
        EditorGUILayout.Space();

        // Stackable Property
        _componentToggles[typeof(StackComponent)] = BeginToggleGroup<StackComponent>("Stackable");
        {
            StackComponent component = GetOrCreateComponent<StackComponent>(item);
            component.MaxQuantity = EditorGUILayout.IntSlider("Max Stack Size", component.MaxQuantity, 0, 99);
        }
        EditorGUILayout.EndToggleGroup();
        EditorGUILayout.Space();

        // Trashable Property
        _componentToggles[typeof(TrashComponent)] = BeginToggleGroup<TrashComponent>("Trashable");
        {
            TrashComponent component = GetOrCreateComponent<TrashComponent>(item);
        }
        EditorGUILayout.EndToggleGroup();
    }

    private bool BeginToggleGroup<T>(string label) where T : IItemComponent
    {
        if (!_componentToggles.ContainsKey<T>()) _componentToggles.Set<T>(_item.GetComponent<T>() != null);

        return EditorGUILayout.BeginToggleGroup(label, _componentToggles.Get<T>());
    }

    private static T GetOrCreateComponent<T>(Item item) where T : IItemComponent, new()
    {
        var component = item.GetComponent<T>();
        if (component == null)
        {
            component = new T();
            item.Components.Add(component);
        }

        return component;
    }

    private void SetBlankItem()
    {
        _filename = "blank.json";
        _itemIcon = null;

        var components = new List<IItemComponent>
        {
            new EquipComponent(), new PotionComponent(), new StackComponent(), new TrashComponent()
        };
        _item = new Item("overtail:blank", "blank", components, "blank.png");
    }

    private void LoadFile(string filepath)
    {
        UnityEngine.Debug.Log($"Loaded item from {filepath}");

        _filename = Path.GetFileName(filepath);

        _item = JsonUtility.FromJson<Item>(System.IO.File.ReadAllText(filepath));

        _componentToggles.Clear();
        foreach (IItemComponent c in _item.Components)
        {
            _componentToggles[c.GetType()] = true;
        }

        _itemIcon = LoadScaledTexture(96);
    }

    private Texture2D LoadScaledTexture(int scale = 128)
    {
        try
        {
            var imgPath = Path.Combine(_directory, _item.SpriteId).Replace("\\", "/").Replace("./", "");

            var texture = AssetDatabase.LoadAssetAtPath<Texture2D>(imgPath);

            Texture2D result = new Texture2D(scale, scale, texture.format, false);
            float incX = (1.0f / scale);
            float incY = (1.0f / scale);

            for (int i = 0; i < result.height; ++i)
            {
                for (int j = 0; j < result.width; ++j)
                {
                    Color newColor = texture.GetPixelBilinear((float) j / (float) result.width,
                        (float) i / (float) result.height);
                    result.SetPixel(j, i, newColor);
                }
            }

            
            result.filterMode = FilterMode.Point;
            result.Compress(false);
            result.Apply();
            return result;
        }
        catch (System.NullReferenceException)
        {
            return null;
        }
    }

    private void SaveFile([NotNull] string filepath)
    {
        FilterComponents(_item, _componentToggles, new Type[]
            {
                typeof(EquipComponent),
                typeof(PotionComponent),
                typeof(StackComponent),
                typeof(TrashComponent)
            }
        );

        File.WriteAllText(filepath, JsonUtility.ToJson(_item));
        UnityEngine.Debug.Log($"Saved item to {filepath}");
    }

    private void FilterComponents(Item item, InterfaceDictionary<IItemComponent, bool> dict, Type[] types)
    {
        foreach (Type type in types)
        {
            if (!dict[type]) item.Components.RemoveAll(c => c.GetType() == type);
        }
    }
}