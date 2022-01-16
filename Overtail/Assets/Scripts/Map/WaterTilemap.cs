using UnityEngine;
using UnityEngine.Tilemaps;

namespace Overtail.Map
{
    /// <summary>
    /// Create a 2D Tilemap object and draw only (swimmable) water tiles there.
    /// Attach this script to it.
    /// Nothing else.
    /// </summary>
    [RequireComponent(typeof(TilemapCollider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(CompositeCollider2D))]
    [DisallowMultipleComponent]
    public class WaterTilemap : MonoBehaviour
    {
        private Tilemap tilemap;

        private TilemapCollider2D collider2d;
        private Rigidbody2D rb2d;
        private CompositeCollider2D compCollider2d;

        // Start is called before the first frame update
        private void Awake()
        {
            tilemap = GetComponent<Tilemap>();
            collider2d = GetComponent<TilemapCollider2D>();
            compCollider2d = GetComponent<CompositeCollider2D>();
            rb2d = GetComponent<Rigidbody2D>();

            Settings();
        }

        private void Settings()
        {
            collider2d.usedByComposite = true;
            rb2d.bodyType = RigidbodyType2D.Static;
        }

        /// <summary>
        /// Enable when idk. Water becomes unpassable again. Collision
        /// </summary>
        public void EnableWater()
        {
            collider2d.enabled = true;
        }

        /// <summary>
        /// Enable when player can swim. Water becomes passable terrain.
        /// </summary>
        public void DisableWater()
        {
            collider2d.enabled = false;
        }

        /// <summary>
        /// Use this to check whether player is standing on deep water.
        /// Example: When you need to know whether to use swimming animation.
        /// </summary>
        /// <param name="playerPos"></param>
        /// <returns></returns>
        public bool HasTile(Vector3 playerPos)
        {
            return tilemap.HasTile(Vector3Int.FloorToInt(playerPos));
        }
    }
}