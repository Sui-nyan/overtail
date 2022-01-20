using Overtail.Map;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Overtail.PlayerModule
{
    public class WaterMovement : MonoBehaviour
    {
        public Tilemap tilemap;
        public bool isInWater { get; private set; }

        private PlayerMovement _movement;

        void Awake()
        {
            _movement = GetComponent<PlayerMovement>();
            tilemap = FindObjectOfType<WaterTilemap>().GetComponent<Tilemap>();
        }
        
        void FixedUpdate()
        {
            isInWater = tilemap.HasTile(Vector3Int.CeilToInt(transform.position));
            _movement.externalMultiplier = isInWater ? .5f : 1;
        }
    }
}
