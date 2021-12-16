using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Overtail.Items;
using Overtail.Battle;
using Overtail;

namespace Overtail
{
    [CreateAssetMenu(fileName = "PlayerData", menuName = "BattleSystem/Create Persistent Player Data (Unique)")]
    [System.Serializable]
    public class PersistentPlayerData : ScriptableObject
    {

        public PlayerSerializable playerSerializable;
    }



}