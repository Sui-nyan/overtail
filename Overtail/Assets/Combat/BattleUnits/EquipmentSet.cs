using System;
using UnityEngine;

/// <summary>
/// Encapsulated whole equipment set for more compact method calls
/// </summary>

[System.Serializable]
public class EquipmentSet
{
    public HeadEquipment head = null;
    public BodyEquipment body = null;
    public Weapon weapon = null;

    public Equipment[] all { get => new Equipment[] { head, body, weapon }; }
}