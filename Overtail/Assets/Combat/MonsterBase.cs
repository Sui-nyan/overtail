using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Monster", menuName = "Monster/Create new base")]
public class MonsterBase : ScriptableObject
{
    [SerializeField] private new string name;
    [SerializeField] [TextArea] private string description;
    [SerializeField] private Sprite sprite;

    // base stats
    [SerializeField] private int maxHealth;
    [SerializeField] private int attack;
    [SerializeField] private int defense;
    [SerializeField] private int speed;

    public string Name { get => name; }
    public string Description { get => description; }
    public Sprite Sprite { get => sprite; }
    public int MaxHealth { get => maxHealth; }
    public int Attack { get => attack; }
    public int Defense { get => defense; }
    public int Speed { get => speed; }
}
