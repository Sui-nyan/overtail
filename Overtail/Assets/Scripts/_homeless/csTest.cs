using System;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System.Linq;


namespace K.SandBox
{
    /// <summary>
    /// Stupid things I have to test because i dont know stuff
    /// </summary>
    public class csTest : MonoBehaviour
    {
        void Start()
        {
            Main();
        }

        void Main()
        {
            // NullInLists();
            // RemoveAllNull();
            if (new ItemStack().Item?.Name == "Blank")
            {
                UnityEngine.Debug.Log("Wtf");
            }
            else
            {
                UnityEngine.Debug.Log("Nothing");
            }
        }

        class A
        {
        }

        // List<T> can contain null
        private void NullInLists()
        {
            UnityEngine.Debug.Log($"> {MethodBase.GetCurrentMethod().Name}()");
            List<A> objList = new List<A>();
            objList.Add(new A());
            objList.Add(null);
            objList.Add(new A());

            UnityEngine.Debug.Log(ListToString(objList));
        }

        private void RemoveAllNull()
        {
            UnityEngine.Debug.Log($"> {MethodBase.GetCurrentMethod().Name}()");
            List<A> objList = new List<A>();
            objList.Add(new A());
            objList.Add(null);
            objList.Add(new A());

            UnityEngine.Debug.Log(ListToString(objList));

            UnityEngine.Debug.Log("RemoveAll()");
            objList.RemoveAll(o => o == null);

            UnityEngine.Debug.Log(ListToString(objList));

        }

        private static string ListToString<T>(List<T> objList)
        {
            if (objList == null) return "NULL LIST";
            return "[" + String.Join(", ", objList.Select(x => x == null ? "null" : x.ToString()).ToArray()) + "]";
        }
        private void NullItem()
        {
            Container c = new Container();
            UnityEngine.Debug.Log(ListToString(c.items));

            c.items = new List<K.SandBox.ItemStack>();
            UnityEngine.Debug.Log(ListToString(c.items));

            c.items = new List<K.SandBox.ItemStack>{new ItemStack(), null, new ItemStack()};
            UnityEngine.Debug.Log(ListToString(c.items));
            foreach (var a in c.items)
            {
                UnityEngine.Debug.Log(a);
            }

        }
    }

    public class Container
    {
        public List<ItemStack> items { get; set; }
    }

    public class ItemStack
    {
        public Item Item { get; set; }

        public ItemStack(Item item)
        {
            this.Item = item;
        }

        public ItemStack()
        {

        }

        public override string ToString()
        {
            return $"[{this.GetType().Name}] {{{(Item == null ? "NULL" : Item.ToString())}}}";
        }
    }

    public class Item
    {
        public string Name { get; set; }
        public string Id { get; set; }

        public Item(string name = "Blank", string id = "0")
        {
            Name = name;
            Id = id;
        }

        public override string ToString()
        {
            return $"[ID:{Id}, Name:{Name}]";
        }
    }
}