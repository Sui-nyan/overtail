using System.Collections.Generic;
using System;
using System.Linq;

namespace Overtail.Collections
{
    public class InterfaceDictionary<I, V>
    {
        Dictionary<string, V> collection = new Dictionary<string, V>();
        public int Count => collection.Count;
        private static string key<T>() where T : I
        {
            return typeof(T).ToString();
        }

        public V this[Type type]
        {
            get { return collection[type.ToString()]; }
            set { collection[type.ToString()] = value; }
        }

        public V Get<T>() where T : I
        {
            try
            {
                return collection[key<T>()];
            }
            catch (KeyNotFoundException)
            {
                throw new KeyNotFoundException($"{key<T>()} could not be found as key");
            }
        }

        public void Set<T>(V value) where T : I
        {
            collection[key<T>()] = value;
        }

        public bool ContainsKey<T>() where T : I
        {
            return collection.ContainsKey(key<T>());
        }

        public void Clear()
        {
            collection.Clear();
        }

        public void Remove<T>() where T : I
        {
            collection.Remove(key<T>());
        }

        public override string ToString()
        {
            return "[" +
                String.Join(", ", collection.Select(i => i.Key + ":" + i.Value));
        }
    }
}