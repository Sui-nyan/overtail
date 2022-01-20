using System.Collections;
using System.Collections.Generic;
using Overtail.Items;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Overtail.Items
{
    public class Chest : MonoBehaviour
    {
        public bool IsOpen { get; private set; }
        public float fadeOutTime = 0.05f;
        [SerializeField] private Image item_image;
        [SerializeField] private TMP_Text item_text;
        [SerializeField] private GameObject _object;

        public string[] itemInChest = {"overtail:felix", "overtail:cat_ears", "overtail:potion"}; //will this be a random pool off items?
        //Would it make sense to categorize items and just get a random item from a specific category? e.g. This chest spawns weapons and this one potions bla bla
        int itemQuantity = 1; //How many Items are in there?
        public SpriteRenderer renderer;

        // Start is called before the first frame update
        void Start()
        {
            renderer = GetComponent<SpriteRenderer>();
        }

        public void OpenChest()
        {
            IsOpen = true;
            StartCoroutine(FadeOut());
            this.gameObject.SetActive(false);
            int i = Random.Range(0, itemInChest.Length - 1);
            GetItem(itemInChest[i]);
        }

        public void GetItem(string item_id)
        {
            Item myItem = ItemDatabase.GetFromId(item_id);
            ItemStack myStack = new ItemStack(myItem, itemQuantity);
            ShowItem(myItem);
        }

        private IEnumerator FadeOut()
        {
            for(float f = 1f; f >= -0.05f; f-=0.05f) 
            {
                Color c = renderer.material.color;
                c.a -= f;
                renderer.material.color = c;
            }
            yield return new WaitForSeconds(fadeOutTime);
        }

        void ShowItem(Item item)
        {
            item_image.sprite = item.Sprite;
            item_text.text = item.Name;
            gameObject.SetActive(true);
        }

        public void Magic()
        {
            //thanks :>

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
}
