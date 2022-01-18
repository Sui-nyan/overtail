using System;
using Overtail.PlayerModule;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Overtail.Battle.Encounter
{
    [System.Serializable]
    internal class Pedometer
    {
        [SerializeField] private float distanceMax = 5;
        [SerializeField] private float distanceWalked;
        [SerializeField] private float tickCooldown = 1f;
        [SerializeField] private float timeSpentWalking;

        private readonly Player _player;
        private readonly Tilemap _tilemap;
        private readonly PlayerMovement _playerMovement;

        internal Pedometer(Player p, Tilemap m)
        {
            UnityEngine.Debug.Log($"[Pedometer] Player::{p}");
            UnityEngine.Debug.Log($"[Pedometer] Tile map::{m}");
            if (m == null) UnityEngine.Debug.LogWarning("Tile map is missing");

            this._player = p;
            this._playerMovement = p.GetComponent<PlayerMovement>();

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
            if (tallGrass)
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
