using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace yours_indie_gameDev.Plugin.DataStructures
{
    public class Observer/*: EventBase*/ { }
    public class SortObserver : Observer { }
    public class RemoveObserver : Observer { }
    public class RemoveAtObserver : Observer { }
    public class AddObserver : Observer { }
    public class OrderByObserver : Observer { }
    public class OrderByDescendingObserver : Observer { }
    public class InsertObserver : Observer { public InsertObserver(int index) { } }
    [Serializable]
    public class ObservedList<T> : IList<T>, IList, ICollection<T>, ICollection, IEnumerable, IEnumerable<T>
    {
        [SerializeField] public List<T> observedList = new();
        HashSet<T> set = new HashSet<T>();
        Action<T, ObserverArgs> listobserver;
        Dictionary<String, ObserverDelegate> observers = new();
        public delegate void ObserverDelegate(T args);
        public ObservedList()
        {
            observedList = new();
        }
        public ObservedList(IEnumerable<T> collection)
        {
            observedList = new();
            foreach (var item in collection)
            {
                observedList.Add(item);
            }
        }
        public ObservedList<T> RegisterObserver<Ev>(ObserverDelegate observer) where Ev : Observer
        {
            if (observers.ContainsKey(typeof(Ev).Name))
            {
                Debug.Log("already subscribed to this event");
                return this;
            }
            observers.Add(typeof(Ev).Name, new ObserverDelegate(observer));//observers[typeof(AddObserver).Name] += (_) => Debug.Log("add");
            return this;
        }
        public T this[int index]
        {
            get => (index < Count) ? observedList[index] : default;
            set
            {
                if (index < Count)
                    observedList[index] = value;
                else observedList.Add(value);
            }
        }
        object IList.this[int index] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public int Count => observedList.Count;

        public bool IsReadOnly => ((IList)observedList).IsReadOnly;

        public bool IsFixedSize => ((IList)observedList).IsFixedSize;

        public bool IsSynchronized => ((IList)observedList).IsSynchronized;

        public object SyncRoot => ((IList)observedList).SyncRoot;


        public void Add(T item)
        {
            observedList.Add(item);
            observers.ValidateKey(typeof(AddObserver).Name)?.Invoke(item);
        }
        public void Insert(int index, T item)
        {
            observedList.Insert(index, item);
            observers[typeof(InsertObserver).Name]?.Invoke(item);
        }
        public bool Remove(T item)
        {
            if (observedList.Remove(item))
            {
                observers.ValidateKey(typeof(RemoveObserver).Name)?.Invoke(item);
                return true;
            }
            return false;
        }
        public void RemoveAt(int index)
        {
            observedList.RemoveAt(index);
            //observers[typeof(RemoveAtObserver).Name]?.Invoke(item);
        }
        public void Clear()
        {
            observedList.Clear();
            listobserver?.Invoke(default, ObserverArgs.Clear);
        }
        public void UpdateItems(T item)
        {
            if (!set.Add(item))
            {
                // Duplicate found
                observedList.Remove(item);
            }
            observedList.Add(item);
        }
        int IList.Add(object value)
        {
            Add((T)value);
            return ((IList)this).IndexOf(value);
        }
        public ObservedList<T> Add(T item, int a = 1)
        {
            Add(item);
            return this;
        }
        public int Length(Predicate<T> match)
        {
            //if (match == null)throw new ArgumentNullException(nameof(match));
            int count = 0;
            observedList.ForEach(item => { if (match(item)) count++; });
            return count;
        }

        public bool Contains(T item) => observedList.Contains(item);
        public bool Contains(object value) => Contains((T)value);

        public void CopyTo(T[] array, int arrayIndex) => observedList.CopyTo(array, arrayIndex);

        public int IndexOf(T item) => observedList.IndexOf(item);
        int IList.IndexOf(object value) => IndexOf((T)value);


        void IList.Insert(int index, object value) => Insert(index, (T)value);




        void IList.Remove(object value) => Remove((T)value);



        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        public IEnumerator<T> GetEnumerator()
        {
            foreach (var item in observedList) yield return item;

        }






        public void CopyTo(Array array, int index) => observedList.CopyTo(array as T[], index);

        //////////////////////
        #region arrangementmanipulation
        public void Sort(IComparer<T> comparer)
        {
            observedList.Sort(comparer);
            observers.ValidateKey(typeof(SortObserver).Name)?.Invoke(default);
        }
        public void Sort(Comparison<T> comparison)
        {
            observedList.Sort(comparison);
            observers.ValidateKey(typeof(SortObserver).Name)?.Invoke(default);
        }
        public void OrderBy<TKey>(Func<T, TKey> keySelector)
        {
            observedList = observedList.OrderBy(keySelector).ToList();
            observers.ValidateKey(typeof(OrderByObserver).Name)?.Invoke(default);
        }
        public void OrderByDescending<TKey>(Func<T, TKey> keySelector)
        {
            observedList = observedList.OrderByDescending(keySelector).ToList();
            observers.ValidateKey(typeof(OrderByDescendingObserver).Name)?.Invoke(default);
        }
        public ObservedList<T> Filter(Predicate<T> match)
        {
            if (this == null) throw new ArgumentNullException("null");
            var matchslots = observedList.Where(item => match(item));
            observedList = matchslots.ToList();
            return this;
        }
        public ObservedList<T> CopyFrom(IEnumerable<T> collection)
        {
            observedList = collection.ToList();
            return this;
        }
        #endregion



    }













}
#region foodforthot

/*

public class Observer<T>
    {
        internal delegate void EventHandler<ObserveArgs>(ObserveArgs observe);
        internal EventHandler<ObserveArgs> listModified;
        public void Notify(ObserveArgs item)
        {
            listModified?.Invoke(item);
        }

    }

    public class AddObserver : Observer<AddObserver>
    {
        public void kNotify(int item)
        {
            //listModified?.Invoke();

        }
    }
    public class ObserveArgs : EventArgs
    {
        public Type type;
    }
private Dictionary<Type, Action<T, ObserverArgs>> observers = new Dictionary<Type, Action<T, ObserverArgs>>();

    public ObservedList<T> RegisterObserver<TObserver>(Action<T, ObserverArgs> observer)
    {
        if (observers.ContainsKey(typeof(TObserver)))
        {
            throw new ArgumentException($"Observer of type {typeof(TObserver)} is already registered.");
        }

        observers[typeof(TObserver)] .Name= observer;
        return this;
    }

    public void NotifyObservers<TObserver>(T item, ObserverArgs mode)
    {
        if (observers.TryGetValue(typeof(TObserver), out var observer))
        {
            observer.Invoke(item, mode);
        }
    }
*/
#endregion