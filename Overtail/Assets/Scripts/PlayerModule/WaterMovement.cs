using System.Collections;
using System.Collections.Generic;
using Overtail.Map;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Overtail.PlayerModule
{
    public class WaterMovement : MonoBehaviour
    {
        public Tilemap tilemap;
        public bool isInWater { get; private set; }
        private PlayerMovement movement;

        void Awake()
        {
            movement = GetComponent<PlayerMovement>();
            tilemap = FindObjectOfType<WaterTilemap>().GetComponent<Tilemap>();
        }
        
        void Update()
        {
            isInWater = tilemap.HasTile(Vector3Int.CeilToInt(transform.position));
            movement.externalMultiplier = isInWater ? .5f : 1;
        }
    }
}


