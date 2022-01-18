using System.Collections;
using System.Collections.Generic;
using Overtail.Items;
using UnityEngine;

public class Chest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if(Input.GetKeyDown(KeyCode.Space)) Magic();
    }

    public void OpenChest()
    {
        //spawnItem();
        //setActive = false chest object despawns
    }
    public void Magic()
    {
        // stupid stuff, but ... maybe some things are  useful, sry

        Item myItem = ItemDatabase.GetFromId("overtail:potion");

        ItemStack myStack = new ItemStack(myItem, 99);

        bool successful = InventoryManager.Instance.PickUp(myStack);
        if (!successful)
        {
            var l = Lootable.Instantiate(myItem, 123, transform.position + new Vector3(Random.value * 5 - 5, Random.value * 5 - 5, 0));
            var rb = l.gameObject.AddComponent<Rigidbody2D>();
            rb.gravityScale = 0;
            Debug.LogWarning("Dropped items");
        }
        else
        {
            Debug.LogWarning("Sent items to inventory");
        }
    }

}
