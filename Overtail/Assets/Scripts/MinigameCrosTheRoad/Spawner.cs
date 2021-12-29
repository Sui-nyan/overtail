using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Car template;
    public float velocity;
    public Vector2 facing;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) SpawnCar();
    }

    private void SpawnCar()
    {
        Car car = Instantiate(template);

        car.velocity = this.velocity;
        car.facing = this.facing;
        car.transform.localPosition = transform.localPosition;
        Debug.Log(car.Size);
    }
}
