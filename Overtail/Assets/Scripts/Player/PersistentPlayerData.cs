using UnityEngine;


namespace Overtail
{
    [CreateAssetMenu(fileName = "PlayerData", menuName = "BattleSystem/Create Persistent Player Data (Unique)")]
    [System.Serializable]
    public class PersistentPlayerData : ScriptableObject
    {

        public PlayerSerializable playerSerializable;
    }



}