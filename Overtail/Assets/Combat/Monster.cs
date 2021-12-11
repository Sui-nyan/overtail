using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{

    [SerializeField] private MonsterBase _base;
    [SerializeField] private int level;
    [SerializeField] private string overrideName;
    [SerializeField] private MonsterAI ai;

    public string Name { get => overrideName != "" ? overrideName : _base.Name; }
    public string Description { get => _base.Description; }
    public int MaxHealth { get => _base.MaxHealth * level; }
    public int Attack { get => _base.Attack * level; }
    public int Defense { get => _base.Defense * level; }
    public int Speed { get => _base.Speed * level; }

    void Start()
    {
        GetComponent<SpriteRenderer>().sprite = _base.Sprite;
        Debug.Log("Starto()");
    }

    // TODO Battle Behavior
}
