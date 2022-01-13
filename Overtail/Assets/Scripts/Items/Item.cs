using System.Collections.Generic;
using System;
using UnityEngine;
using System.IO;

namespace Overtail.Items
{
    /// <summary>
    /// Data class with exception of method <see cref="GetComponent{T}"/>
    /// </summary>
    [System.Serializable]
    public class Item
    {
        public string Id;
        public string Name = "";
        public string Description = "";

        [SerializeReference] public List<IItemComponent> Components = new List<IItemComponent>();
        public string SpriteId;

        private Item()
        {
        }

        public Item(string id, string name = null, List<IItemComponent> components = null, string spriteId = null, string description = null)
        {
            Id = id;
            Name = name ?? Id;
            SpriteId = spriteId ?? "";
            if (components != null) this.Components = components;
            Description = description ?? "";
        }

        /// <summary>
        /// Method to get component
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetComponent<T>() where T : IItemComponent
        {
            foreach (IItemComponent c in Components)
            {
                try
                {
                    return (T) c;
                }
                catch (System.InvalidCastException)
                {
                }
            }

            return default(T);
        }

        public override bool Equals(object obj)
        {
            if (this == obj) return true;

            try
            {
                return this.Equals(obj as Item);
            }
            catch (InvalidCastException)
            {
                return false;
            }
        }

        public bool Equals(Item other)
        {
            return this.Id == other.Id;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode(); // TODO idk
        }

        public override string ToString()
        {
            return this.Id;
        }
    }
}