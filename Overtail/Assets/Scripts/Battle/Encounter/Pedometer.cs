using System;
using Overtail.PlayerModule;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Overtail.Battle.Encounter
{
    [System.Serializable]
    internal class Pedometer
    {
        [SerializeField] public float distanceMax;
        [SerializeField] public float distanceWalked;
        [SerializeField] public float tickCooldown;
        [SerializeField] public float timeSpentWalking;

        [SerializeField] public bool alwaysGrass;

        private Player _player;
        private Tilemap _tilemap;
        private PlayerMovement _playerMovement;

        internal Pedometer(Player p, Tilemap m)
        {
            UnityEngine.Debug.Log($"[Pedometer] Player::{p}");
            UnityEngine.Debug.Log($"[Pedometer] Tile map::{m}");
            if (m == null) UnityEngine.Debug.LogWarning("Tile map is missing");

            this._player = p;
            this._playerMovement = p.GetComponent<PlayerMovement>();

            this._tilemap = m;
        }

        public void SetPlayer(Player p)
        {
            this._player = p;
            this._playerMovement = p.GetComponent<PlayerMovement>();
        }

        public void SetMap(Tilemap m)
        {
            this._tilemap = m;
        }

        public event Action<float, float> EventTick;

        public void Reset()
        {
            distanceWalked = 0;
            timeSpentWalking = 0;
        }

        // Not a mono behaviour, call from outside please.
        public void FixedUpdate()
        {
            Vector2 pos = _player.transform.position;

            bool isMoving = _playerMovement.IsMoving;
            if (!isMoving) return;

            bool tallGrass = _tilemap != null && _tilemap.HasTile(Vector3Int.FloorToInt(pos));
            if (tallGrass || alwaysGrass)
            {
                timeSpentWalking += Time.fixedDeltaTime;
                distanceWalked += _playerMovement.CurrentMoveSpeed * Time.fixedDeltaTime;

                if (timeSpentWalking > tickCooldown)
                {
                    timeSpentWalking = 0;
                    EventTick?.Invoke(distanceWalked, distanceMax);
                }
            }
        }
    }
}