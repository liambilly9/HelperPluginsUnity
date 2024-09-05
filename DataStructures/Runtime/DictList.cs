using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;
namespace yours_indie_gameDev.Plugin.DataStructures
{
    public enum ObserverArgs { Add, Clear }
    [Serializable]
    public class DictList<K, V> : IFormattable, ICollection<DictItem<K, V>>, IDictionary, IDictionary<K, V>, IReadOnlyDictionary<K, V>, IEnumerable
    {
        [SerializeField] List<DictItem<K, V>> dictItems = new();
        public delegate void ObserverDelegate(K args, V args2);
        Dictionary<string, ObserverDelegate> observers = new();
        public DictList()
        { }
        public DictList<K, V> RegisterObserver<Ev>(ObserverDelegate observer) where Ev : Observer
        {
            if (observers.ContainsKey(typeof(Ev).Name))
            {
                Debug.Log("already subscribed to this event");
                return this;
            }
            observers.Add(typeof(Ev).Name, new ObserverDelegate(observer));
            return this;
        }
        public void Add(DictItem<K, V> item) => Add(item.key, item.value);
        public DictList<K, V> Add(K key, V value)
        {
            if (ContainsKey(key))
            {
                //Debug.LogError((this.ToString() + " already contains Key" + key + " item not added").Colorize(Color.yellow));
                return this;
            }
            DictItem<K, V> item = new(key, value);
            dictItems.Add(item);
            observers.ValidateKey(typeof(AddObserver).Name)?.Invoke(key, value);
            return this;
        }
        public bool ContainsKey(K key)
        {
            return dictItems.Find((item) => item.key.Equals(key)) != null;
        }
        public bool Contains(DictItem<K, V> item)
        {
            return this[item] != null;
        }
        public bool Remove(DictItem<K, V> item) => Remove(item.key, item.value);
        public bool Remove(K key, V value)
        {
            if (!ContainsKey(key)) throw new Exception(this.ToString() + " doesnt contains Key" + key);
            DictItem<K, V> item = this[key, value];
            dictItems.Remove(item);
            observers.ValidateKey(typeof(RemoveObserver).Name)?.Invoke(key, value);
            return true;
        }
        public void RemovePair(K key)
        {
            if (!ContainsKey(key)) throw new Exception(this.ToString() + " doesnt contains Key" + key);
            var value = this[key];
            DictItem<K, V> item = this[key, value];
            dictItems.Remove(item);
        }
        public void RemoveAt(K key)
        {
            if (!ContainsKey(key)) throw new Exception(this.ToString() + " doesnt contains Key" + key);
            this[key] = default;
        }
        public void Clear() => dictItems.Clear();
        public int Count => dictItems.Count;

        public bool IsReadOnly => ((ICollection<DictItem<K, V>>)dictItems).IsReadOnly;

        public IEnumerable<K> Keys => throw new NotImplementedException();

        public IEnumerable<V> Values => throw new NotImplementedException();

        ICollection<K> IDictionary<K, V>.Keys => throw new NotImplementedException();

        ICollection<V> IDictionary<K, V>.Values => throw new NotImplementedException();

        public bool IsFixedSize => throw new NotImplementedException();

        ICollection IDictionary.Keys => throw new NotImplementedException();

        ICollection IDictionary.Values => throw new NotImplementedException();

        public bool IsSynchronized => throw new NotImplementedException();

        public object SyncRoot => throw new NotImplementedException();

        public object this[object key] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public DictItem<K, V> this[K key, V val]
        {
            get => dictItems.Find((v) => v.key.Equals(key));
            set
            {
                var existingItem = dictItems.Find((v) => v.key.Equals(key));
                if (existingItem != null) existingItem.value = val;
                else Add(key, val);
            }
        }
        public DictItem<K, V> this[DictItem<K, V> dictItem]
        {
            get => dictItems.Find((item) => item.Equals(dictItem));

        }

        public V this[K key]
        {
            get => dictItems.Find((item) => item.key.Equals(key)).value;
            set
            {
                var existingItem = dictItems.Find((item) => item.key.Equals(key));
                existingItem.value = value;
            }
        }
        public V ValidateKey(K key)
        {
            if (ContainsKey(key))
            {
                return this[key];
            }
            return default;
        }
        public void CopyFromPairCollection<T>(T icollection) where T : ICollection<KeyValuePair<K, V>>
        {
            foreach (KeyValuePair<K, V> pair in icollection)
            {
                Add(pair.Key, pair.Value);
            }
        }
        public void CopyFromCollection<T>(T collection) where T : ICollection<V>
        {
            int index = -1;
            foreach (var item in collection)
            {
                K key = KeyConverter(index++);
                Add(key, item);
            }
        }
        public K KeyConverter(int index)
        {
            return (K)(object)index;
        }
        //Func<int, string> keyGenerator = index => "Key_" + index;
        public void CopyTo(DictItem<K, V>[] array, int arrayIndex)
        {
            dictItems.CopyTo(array, arrayIndex);
        }
        public string ToString(string format, IFormatProvider formatProvider)
        {
            return null;
        }


        public IEnumerator<DictItem<K, V>> GetEnumerator()
        {
            foreach (var item in dictItems)
            {
                yield return item;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public bool TryGetValue(K key, out V value)
        {
            value = ContainsKey(key) ? this[key] : default;
            return ContainsKey(key);
        }

        IEnumerator<KeyValuePair<K, V>> IEnumerable<KeyValuePair<K, V>>.GetEnumerator()
        {
            foreach (var item in dictItems)
            {
                yield return item.ToKeyValuePair();
            }
        }

        void IDictionary<K, V>.Add(K key, V value)
        {
            throw new NotImplementedException();
        }

        public bool Remove(K key)
        {
            throw new NotImplementedException();
        }

        public void Add(KeyValuePair<K, V> item)
        {
            Add(item.Key, item.Value);
        }

        public bool Contains(KeyValuePair<K, V> item)
        {
            return Contains(new DictItem<K, V>(item.Key, item.Value));
        }

        public void CopyTo(KeyValuePair<K, V>[] array, int arrayIndex)
        {
            DictItem<K, V>[] dictarr = new DictItem<K, V>[array.Length];
            CopyTo(dictarr, arrayIndex);
        }

        public bool Remove(KeyValuePair<K, V> item)
        {
            return Remove(new DictItem<K, V>(item.Key, item.Value));
        }

        public void Add(object key, object value)
        {
            throw new NotImplementedException();
        }

        public bool Contains(object key)
        {
            throw new NotImplementedException();
        }

        IDictionaryEnumerator IDictionary.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public void Remove(object key)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }
    }
    [Serializable]
    public class DictItem<K, V>
    {
        public K key;
        public V value;

        public DictItem(K key, V value)
        {
            this.key = key;
            this.value = value;
        }
        public KeyValuePair<K, V> ToKeyValuePair()
        {
            KeyValuePair<K, V> pair = new(key, value);
            return pair;
        }
        public DictItem() { }
    }
    static class Exts
    {
        #region Dictionary
        static public V ValidateKey<K, V>(this Dictionary<K, V> dict, K key)
        {
            if (dict.ContainsKey(key))
            {
                return dict[key];
            }
            return default;
        }
        #endregion
        #region tocustom
        public static ObservedList<TArgs> ToObservedList<TArgs>(this IEnumerable<TArgs> enumerable)
        {
            ObservedList<TArgs> oblist = new(enumerable);
            return oblist;
        }
        #endregion
    }
}
//to investigate
/*
public delegate TDestination TypeConverter<TSource, out TDestination>(ref TSource value);
    TypeConverter<string, int> typeConverter;
    private void Start()
    {
        typeConverter += co;
    }

    private int co(ref string value)
    {
        return 0;
    }
    */