using UnityEngine;

namespace Overtail.PlayerModule
{
    public class PlayerAnimation : MonoBehaviour
    {
        private PlayerMovement movement;
        private WaterMovement swimming;
        private SpriteRenderer renderer;

        private Sprite sprite_water;
        private Sprite sprite_land;

        private void Awake()
        {
            movement = GetComponent<PlayerMovement>();
            swimming = GetComponent<WaterMovement>();
            renderer = GetComponent<SpriteRenderer>();
        }

        private void Update()
        {

            // TODO: Sprite
            if (movement.direction.x < 0)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }

            if (movement.direction.x > 0)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }

            SetSnorkle();
        }


        void SetSnorkle()
        {
            if (swimming.isInWater)
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