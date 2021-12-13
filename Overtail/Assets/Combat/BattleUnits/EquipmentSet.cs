using System;
using UnityEngine;

[System.Serializable]
public class EquipmentSet
{
    public HeadEquipment head = null;
    public BodyEquipment body = null;
    public Weapon weapon = null;

    public Equipment[] all { get => new Equipment[] { head, body, weapon }; }
}