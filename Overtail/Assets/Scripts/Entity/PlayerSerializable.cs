using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Overtail.Items;
using Overtail.Serializable;
// data class => JSON

namespace Overtail.Player
{

    [System.Serializable]
    public class PlayerSerializable
    {
        [Header("General")]
        [SerializeField] private PlayerStatus status;

        [Header("Overworld")]
        [SerializeField] private Vec3 pos;

        [Header("Inventory")]
        [SerializeField] private Inventory inventory;
        [SerializeField] private EquipmentSet equipment;



        public Inventory Inventory => inventory;
        public EquipmentSet Equipment => equipment;
        public Vector3 Position
        {
            get => new Vector3(pos.x, pos.y, pos.z);
            set => pos = new Vec3(value.x, value.y, value.z);
        }



    }

}

namespace Overtail.Serializable
{
    [System.Serializable]
    class Vec3
    {
        [SerializeField] internal float x = 0;
        [SerializeField] internal float y = 0;
        [SerializeField] internal float z = 0;
        internal Vec3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
    }

    [System.Serializable]
    class PlayerStatus
    {
        [SerializeField] internal int level;
        [SerializeField] internal int exp;
        [SerializeField] internal int hp;
        [SerializeField] internal int maxHp;
        [SerializeField] internal int atk;
        [SerializeField] internal int def;
    }
}