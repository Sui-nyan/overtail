using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Overtail.Arcade.Street
{
    public class CarSpawner : MonoBehaviour
    {
        public enum Direction
        {
            Left, Right, Up, Down
        }

        public Car template;
        public float carSpeed;
        public Direction Facing = Direction.Right;
        public float elapsedTime;

        public float minSpawnDelay;
        public float maxSpawnDelay;

        public float minCarSpeed;
        public float maxCarSpeed;
        private Vector2 facing
        {
            get
            {
                switch (Facing)
                {
                    case Direction.Left:
                        return new Vector2(-1, 0);
                    case Direction.Right:
                        return new Vector2(1, 0);
                    case Direction.Up:
                        return new Vector2(0, 1);
                    case Direction.Down:
                        return new Vector2(0, -1);
                    default:
                        return Vector2.zero;
                }
            }
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            elapsedTime += Time.fixedDeltaTime;
            if (Input.GetKeyDown(KeyCode.Space)) SpawnCar();

            if (elapsedTime > UnityEngine.Random.Range(minSpawnDelay, maxSpawnDelay))
            {
                elapsedTime = 0;
                SpawnCar();
            }
        }

        private void SpawnCar()
        {
            Car car = Instantiate(template);

            car.gameObject.transform.SetParent(this.transform);

            car.velocity = UnityEngine.Random.Range(minCarSpeed, maxCarSpeed);
            car.facing = this.facing;
            car.transform.localPosition = Vector3.zero;
            car.maxDistance = 50f;
        }

        internal void Reset()
        {
            elapsedTime = 0;
        }
    }
}