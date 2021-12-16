using UnityEngine;

namespace Overtail.Items
{
    [System.Serializable]
    public class Item
    {
        [SerializeField] private string name;
        [SerializeField] [TextArea] private string description;
        [SerializeField] bool stackable;

        public string Name => name;
        public string Description => description;
        public bool Stackable => stackable;

        protected Item() { }
        public Item(string name, string description, bool stackable)
        {
            this.name = name;
            this.description = description;
            this.stackable = stackable;
        }
        public Item(string name, string description) : this(name, description, true) { }
        public Item(string name) : this(name, "") { }
        public Item(string name, bool stackable) : this(name, "", stackable) { }

        public override bool Equals(object obj)
        {
            Debug.Log(obj.GetType());
            Item item = obj as Item;
            return (this.name == item.name) &&
                (this.description == item.description) &&
                (this.stackable == item.stackable);
        }

        public override string ToString()
        {
            return $"[{this.name}] {this.description.Substring(0, Mathf.Min(this.description.Length, 20))}";
        }
    }
}