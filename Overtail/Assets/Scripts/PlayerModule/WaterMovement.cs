using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Overtail.PlayerModule
{
    public class WaterMovement : MonoBehaviour
    {
        public Tilemap tilemap;
        public Sprite sprite_water;
        public Sprite sprite_land;
        public bool isInWater { get; private set; }
        public SpriteRenderer renderer;


        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            Debug.Log(isInWater);
            isInWater = tilemap.HasTile(Vector3Int.CeilToInt(transform.position));
            SetSnorkle();
        }

        void SetSnorkle()
        {
            if (isInWater)
            {
                renderer.sprite = sprite_water;
            }
            else
            {
                renderer.sprite = sprite_land;
            }
        }
    }
}


