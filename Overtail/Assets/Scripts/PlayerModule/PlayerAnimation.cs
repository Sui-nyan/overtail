using UnityEngine;

namespace Overtail.PlayerModule
{
    public class PlayerAnimation : MonoBehaviour
    {
        private PlayerMovement _movement;
        private WaterMovement _swimming;
        private SpriteRenderer _renderer;

        private Sprite sprite_water;
        private Sprite sprite_land;

        private void Awake()
        {
            _movement = GetComponent<PlayerMovement>();
            _swimming = GetComponent<WaterMovement>();
            _renderer = GetComponent<SpriteRenderer>();
        }

        private void Update()
        {

            // TODO: Sprite
            if (_movement.direction.x < 0)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }

            if (_movement.direction.x > 0)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }

            SetSnorkle();
        }


        void SetSnorkle()
        {
            if (_swimming.isInWater)
            {
                _renderer.sprite = sprite_water;
            }
            else
            {
                _renderer.sprite = sprite_land;
            }
        }
    }
}
