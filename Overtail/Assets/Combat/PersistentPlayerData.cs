using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "BattleSystem/Create Persistent Player Data (Unique)")]
public class PersistentPlayerData : ScriptableObject
{
    public EntityTemplate template;

    public int level;
    public int currentHealth;
    public Vec3 position;

    public EquipmentSet equipment;
    public List<Buff> buffs = new List<Buff>();
}

[System.Serializable]
public class Vec3
{
    public float x = 0;
    public float y = 0;
    public float z = 0;

    public void SetPosition(Vector3 v)
    {
        this.x = v.x;
        this.y = v.y;
        this.z = v.z;
    }

    public Vector3 GetPosition()
    {
        return new Vector3(x, y, z);
    }
}

