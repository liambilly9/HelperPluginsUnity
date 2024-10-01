using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.UIElements.VisualElement;
using Object = UnityEngine.Object;
namespace yours_indie_gameDev.Plugin.Extensions
{
    public static class Extensions
    {
        #region String
        static public string Unclone(this string name)
        {
            ReadOnlySpan<char> namespan = name;
            var indexclone = namespan.IndexOf('(');
            var edited = namespan.Slice(0, indexclone);
            return edited.ToString();
        }
        public static String Random(this string source, int length, char min = 'a', char max = 'z')
        {//indexes ;special chars#.*, numbers, letters, special chars[]?
            if (min > max)
            {
                throw new Exception("min can't be greater than max");
            }
            StringBuilder sb = new();//Regex
            System.Random random = new();
            for (int i = 0; i < length; i++)
            {
                char randomChar = (char)random.Next(min, max + 1);
                sb.Append(randomChar);
            }
            return sb.ToString();
        }
        public static int ToInt(this string source)
        {
            int val;
            if (int.TryParse(source, out val)) return val;
            else
            {
                if (source == null) return 0;
                StringBuilder sb = new();
                foreach (char c in source)
                {
                    if (char.IsNumber(c)) sb.Append(c);
                    else Debug.Log(c);
                }
                return int.Parse(sb.ToString());
            }
            throw new SystemException("invalid int format");
        }
        public static float ToFloat(this string source)
        {
            float val;
            if (float.TryParse(source, out val)) return val;
            else
            {
                if (source == null) return 0;
                StringBuilder sb = new();
                foreach (char c in source)
                {
                    if (char.IsDigit(c) || c == '.') sb.Append(c);
                    else Debug.Log(c);
                }
                return float.Parse(sb.ToString());
            }
            throw new SystemException("invalid float format");
        }
        public static int ToInt(this ReadOnlySpan<char> source)
        {
            int val;
            if (int.TryParse(source, out val)) return val;
            else
            {
                if (source == null) return 0;
                StringBuilder sb = new();
                foreach (char c in source)
                {
                    if (char.IsNumber(c)) sb.Append(c);
                    else Debug.Log(c);
                }
                return int.Parse(sb.ToString());
            }
            throw new SystemException("invalid int format");
        }
        static public string Bold<T>(this T text) where T : IConvertible => $"<b>{text}</b>";
        static public string Italic<T>(this T text) where T : IConvertible => $"<i>{text}</i>";
        static public string Colorize<T>(this T text, Color color) where T : IConvertible => $"<color=#{ColorUtility.ToHtmlStringRGB(color)}>{text}</color>";
        #endregion

        #region Vectors
        public static Quaternion ToEulers(this Vector3 vector3) => Quaternion.Euler(vector3.x, vector3.y, vector3.z);
        #endregion

        #region Component
        /// <summary>
        /// ActivatesDeactivates the GameObject with this component, depending on the given true or false/ value.
        /// </summary>
        static public T ActiveSelf<T>(this T component, bool active) where T : Component
        {
            component.transform.gameObject.SetActive(active);
            return component;
        }
        /// <summary>
        /// The local active state of this GameObject. (Read Only)
        /// </summary>
        static public bool IsActive<T>(this T component) where T : Component
        {
            return component.transform.gameObject.activeSelf;
        }

        static public T Logger<T>(this T component, LogType logType, object message, Object context = null) where T : Component
        {
            Debug.unityLogger.Log(logType, message, context ?? null);
            return component;
        }
        #endregion


        #region Array
        static public void Clear<T>(this T[] array)
        {
            if (array.Length > 0) Array.Clear(array, 0, array.Length);
        }
        public static int IndexOf<T>(this T[] array, T item) => Array.IndexOf(array, item);
        static public T[] ForEach<T>(this T[] array, Action<T> action)
        {
            if (array.Length > 0) Array.ForEach(array, action); else Debug.Log("length is 0");
            return array;
        }
        public static T Find<T>(this T[] array, Predicate<T> match) => Array.Find(array, match);
        #endregion
        #region list
        static public IList<T> Add<T>(this IList<T> list, T item)
        {
            list.Add(item);
            return list;
        }
        #endregion
        #region Enum
        // static public Enum ToEnum<T>(this int dataindex) where T : Enum
        // {
        //     return ;
        // }
        #endregion

        #region Visualelement

        public static T Enabled<T>(this T ve, bool enabled) where T : VisualElement
        {
            ve.pickingMode = enabled ? PickingMode.Position : PickingMode.Ignore;
            ve.SetEnabled(enabled);
            return ve;
        }
        public static T SetVisible<T>(this T ve, bool visible) where T : VisualElement
        {
            DisplayStyle displayStyle = visible ? DisplayStyle.Flex : DisplayStyle.None;
            ve.style.display = displayStyle;
            return ve;
        }
        public static T OnVisibleChanged<T>(this T ve, EventCallback<ChangeEvent<bool>> eventCallback) where T : VisualElement
        {///incomplete, to test
            //ve.RegisterCallback(eventCallback);
            return ve;
        }
        public static bool IsVisible<T>(this T ve) where T : VisualElement => ve.resolvedStyle.display == DisplayStyle.Flex;
        public static VisualElement Root(this VisualElement ve)
        {
            VisualElement parent = ve.parent;
            while (parent?.parent != null)
            {
                parent = parent.parent;
            }
            return parent ?? ve;
        }
        public static VisualElement CreateChildWithClass(this VisualElement parent, params string[] classes)
        {
            var child = new VisualElement();
            parent.Append(child.AddClasses(classes));
            return child;
        }

        public static T CreateChildWithClass<T>(this VisualElement parent, params string[] classes) where T : VisualElement, new()
        {
            T child = new();
            child.AddClasses(classes).AddTo(parent);
            return child;
        }
        public static UQueryBuilder<T> CreateChildren<T>(this VisualElement parent, int children, params string[] classes) where T : VisualElement, new()
        {
            UQueryBuilder<T> ves = new();
            for (int i = 0; i < children; i++)
            {
                T child = new();
                if (classes.Length > 0) child.AddClasses(classes);
                parent.Append(child);
                ves.Children<T>();
            }
            return ves;
        }
        public static T[] CreateChildren<T>(this VisualElement parent, int children) where T : VisualElement, new()
        {
            T[] ves = new T[children];
            for (int i = 0; i < children; i++)
            {
                T child = new();
                parent.Append(child);
                ves[i] = child;
            }
            return ves;
        }
        public static T AddTo<T>(this T child, VisualElement parent) where T : VisualElement
        {
            parent.Add(child);
            return child;
        }
        public static T Append<T>(this T parent, VisualElement child) where T : VisualElement
        {
            parent.Add(child);
            return parent;
        }
        public static T AddClasses<T>(this T visualElement, params string[] classes) where T : VisualElement
        {
            foreach (string cls in classes)
            {
                if (!string.IsNullOrEmpty(cls))
                {
                    visualElement.AddToClassList(cls);
                }
            }
            return visualElement;
        }
        public static T AddClass<T>(this T visualElement, string ussclass) where T : VisualElement
        {
            visualElement.AddToClassList(ussclass);
            return visualElement;
        }
        public static T WithManipulator<T>(this T visualElement, IManipulator manipulator) where T : VisualElement
        {
            visualElement.AddManipulator(manipulator);
            return visualElement;
        }
        public static VisualElement WithCallBack<T>(this VisualElement visualElement, EventCallback<T> eventCallback) where T : EventBase<T>, new()
        {
            visualElement.RegisterCallback(eventCallback);
            return visualElement;
        }
        public static V WithCallBack<V, T>(this V ve, EventCallback<T> eventCallback) where T : EventBase<T>, new()
        where V : VisualElement
        {
            ve.RegisterCallback(eventCallback);
            return ve;
        }
        public static Hierarchy Append(this Hierarchy parent, VisualElement child)
        {
            parent.Add(child);
            return parent;
        }
        public static T[] ToArray<T>(this UQueryBuilder<T> uqb) where T : VisualElement => uqb.ToList().ToArray();
        public static T[] ToArray<T>(this UQueryBuilder<T> uqb, Func<T, T> predicate = null) where T : VisualElement
        {
            uqb.ToList().ForEach(s => predicate?.Invoke(s));
            return uqb.ToList().ToArray();
        }
        public static T[] ToArray<T>(this UQueryBuilder<T> uqb, Action predicate = null) where T : VisualElement
        {
            uqb.ToList().ForEach(s => predicate?.Invoke());
            return uqb.ToList().ToArray();
        }
        public static T DisableChildren<T>(this T ve, bool disableSelf = false) where T : VisualElement
        {
            foreach (var child in ve.Children())
            {
                child.pickingMode = PickingMode.Ignore;
                DisableChildren(child);
            }
            if (disableSelf) ve.pickingMode = PickingMode.Ignore;
            return ve;
        }
        public static T FindClosestOfType<T>(this VisualElement ve, T[] ves) where T : VisualElement
        {
            T closestVe = ves.Where(_ve => _ve.worldBound.Overlaps(ve.worldBound))
            .OrderBy(_ve => Vector2.Distance(_ve.worldBound.position, ve.worldBound.position))
            .FirstOrDefault();
            return closestVe;
        }
        #endregion

        #region Type
        static public bool IsCustomClass(this Type thisType)
        {
            if (typeof(Component).IsAssignableFrom(thisType)) return false;
            //if (thisType.GetInterface("ISaveable")!=null) return false;//todov3;
            bool isbBuiltinClass = thisType.Namespace == nameof(System) || thisType.Namespace == nameof(UnityEngine)
            || thisType.Namespace == "Unity.Netcode" || thisType.IsCollectionType();//string,transform e.t.c
            bool customClass = thisType.IsClass && !isbBuiltinClass;//(!thisType.IsValueType && !isbBuiltinClass)
            return customClass;
        }

        static public bool IsComponent(this object inst)
        {
            if (inst is Component) return true;
            else if (inst is Object) return true;//component is also Obj,
            return false;
        }
        public static bool IsCollectionType(this Type type) => typeof(IEnumerable).IsAssignableFrom(type) && type != typeof(string);

        static public bool IsCustomStruct(this Type thisType)
        {
            bool isBuiltinStruct = thisType.Namespace == nameof(System) || thisType.Namespace == nameof(UnityEngine) ||
             thisType.Namespace == "Unity.Mathematics" || thisType.IsEnum;
            //string,transform e.t.c how about networkvariables
            bool customStruct = thisType.IsValueType && !isBuiltinStruct;
            return customStruct;
        }
        public static Type ReturnType(this MemberInfo memberInfo)
        {//memberInfo.GetAccessorType();
            if (memberInfo is FieldInfo field) return field.FieldType;
            else if (memberInfo is PropertyInfo property) return property.PropertyType;
            else if (memberInfo is MethodInfo method) return method.ReturnType;
            return default;
        }

        public static MemberInfo SetVal<T>(this T member, object instance, object value) where T : MemberInfo
        {
            if (member is FieldInfo field)
                field.SetValue(instance, value);
            else if (member is PropertyInfo property)
                property.SetValue(instance, value);
            return member;
        }

        private static object Value<T>(this T member, object instance, object value) where T : MemberInfo
        {
            if (member is FieldInfo field)
                return field.GetValue(instance);
            else if (member is PropertyInfo property)
                return property.GetValue(instance);
            return null;
        }
        public static Type GetCollectionArgType(this Type collectionType)
        {
            if (collectionType == null) return null;
            if (collectionType.IsArray)
            {
                return collectionType.GetElementType();
            }

            Type[] genericArguments = collectionType.GetGenericArguments();

            if (genericArguments.Length > 0)
            {
                return genericArguments[0];
            }

            return null; //todo; Handle other collection types as needed e.g 2d arrays
        }
        public static int GetDimensionCount(this Type type)
        {
            // Handle arrays
            if (type == null) throw new Exception("null ref");
            if (type.IsArray)
            {
                return type.GetArrayRank();
            }

            // Handle generic collections (e.g., List<T>, Dictionary<TKey, TValue>)
            if (type.IsGenericType && typeof(IEnumerable).IsAssignableFrom(type))
            {
                int depth = 1;
                Type genericArgument = type.GetGenericArguments()[0];

                // Recursively check for nested collections
                while (typeof(IEnumerable).IsAssignableFrom(genericArgument) && genericArgument != typeof(string))
                {
                    depth++;
                    if (genericArgument.IsGenericType)
                    {
                        genericArgument = genericArgument.GetGenericArguments()[0];
                    }
                    else if (genericArgument.IsArray)
                    {
                        genericArgument = genericArgument.GetElementType();
                    }
                    else
                    {
                        break;
                    }
                }

                return depth;
            }

            // Handle non-generic collections (e.g., ArrayList)
            if (typeof(IEnumerable).IsAssignableFrom(type))
            {
                return 1;
            }

            // If it's not a collection, return 0
            return 0;
        }

        #endregion
        #region collecion
        public static void ForEach(this IEnumerable collection, Action<object> action)
        {
            foreach (var item in collection)
            {
                action(item);
            }
        }
        public static Array ConvertToArray(this IEnumerable from)
        {
            var elementType = from.GetType().GetGenericArguments()[0];

            // Get the `ToArray` method from `Enumerable`
            var toArrayMethod = typeof(Enumerable)
                .GetMethod("ToArray")
                .MakeGenericMethod(elementType);

            // Invoke the `ToArray` method on the `from` collection
            return (Array)toArrayMethod.Invoke(null, new object[] { from });
        }
        public static Type GetTypeDefinition(this IEnumerable collection)
        {
            Type collectionType = collection.GetType();
            if (collectionType.IsArray) { Debug.Log("array"); return typeof(Array); }
            else if (collectionType.IsGenericType) { Debug.Log("list"); return collectionType.GetGenericTypeDefinition(); }
            Debug.Log("neither");
            return default;
        }
        public static int Get1DCollectionSize(this IEnumerable collection)
        {
            // Handle List<T> or any collection implementing ICollection
            if (collection == null) return 0;
            var collectionType = collection.GetType();
            if (typeof(ICollection).IsAssignableFrom(collectionType) && collectionType.IsGenericType)
            {
                return (int)collectionType.GetProperty("Count").GetValue(collection);
            }

            // Handle arrays
            else if (collectionType.IsArray)
            {
                return (int)collectionType.GetProperty("Length").GetValue(collection);
            }

            throw new InvalidOperationException("The collection is neither an ICollection nor an array.");
        }
        public static int GetIndexOfItem(this object collection, object item)
        {
            if (collection is IList list)
            {
                return list.IndexOf(item);
            }
            else if (collection is Array array)
            {
                for (int i = 0; i < array.Length; i++)
                {
                    if (Equals(array.GetValue(i), item))
                    {
                        return i;
                    }
                }
            }
            else
            {
                throw new InvalidOperationException("This system supports onlt lists and arrayas.");
            }

            return -1; // Return -1 if item is not found
        }
        static public V ValidateKey<K, V>(this Dictionary<K, V> dict, K key)
        {
            if (dict.ContainsKey(key))
            {
                return dict[key];
            }
            return default;
        }
        public static bool TryGet<T>(this IList<T> list, Predicate<T> match, out T item)
        {
            item = default;
            foreach (var x in list)
            {
                if (match(x))
                {
                    item = x;
                    return true;
                }
            }
            return false;
        }
        public static T TryGet<T>(this IList<T> list, Predicate<T> match)
        {
            foreach (var x in list)
            {
                if (match(x))
                {
                    return x;
                }
            }
            return default;
        }
        public static bool TryGetAt<T>(this IList<T> list, int index, out T item)
        {
            if (index < list.Count && list[index] != null)
            {
                item = list[index];
                return true;
            }
            item = default;
            return false;
        }
        public static T LastFromDepthOrDefault<T>(this List<T> list, int depth = 1) where T : class
        {
            int nonNullCount = 0;
            for (int i = list.Count - 1; i >= 0; i--)
            {
                T item = list[i];
                //Debug.Log(list[i].ToString());
                if (!list[i].Equals(null))
                {
                    nonNullCount++;
                    if (nonNullCount == depth)
                    {
                        return item;
                    }
                }

            }
            Debug.Log("No non-null item found.");
            return default;
        }
        public static T? LastFromDepthOrDefault<T>(this List<T?> list, int depth = 1) where T : struct
        {
            //not tested
            Debug.Log(typeof(T));
            int nonNullCount = 0;
            for (int i = list.Count - 1; i >= 0; i--)
            {
                if (list[i].HasValue)
                {
                    nonNullCount++;
                    if (nonNullCount == depth)
                    {
                        Debug.Log($"Found non-null item at Index {i}: {list[i].Value}");
                        return list[i].Value;
                    }
                }

            }
            Debug.Log("No non-null item found.");
            return default;
        }
        public static T AddItem<T>(this List<T> list, T item)
        {
            list.Add(item);

            return item;
        }

        public static List<T> Update<T>(this List<T> list)
        {
            for (int i = list.Count - 1; i >= 0; i--)
            {
                if (list[i].Equals(null))
                {
                    list.RemoveAt(i);
                }
            }
            return list;
        }
        public static T[] FindAll<T>(this Span<T> span, Func<T, bool> predicate)
        {
            // Create an array to store the results
            T[] result = new T[span.Length];
            int index = 0;

            // Fill the result array with matching items
            for (int i = 0; i < span.Length; i++)
            {
                if (predicate(span[i]))
                {
                    result[index++] = span[i];
                }
            }

            // If index < span.Length, ignore the empty slots
            // Alternatively, you can leave this as is and return the array.
            return result.AsSpan(0, index).ToArray();
        }
        public static T FindAsSpan<T>(this List<T> list, Predicate<T> predicate)
        {
            Span<T> span = list.ToArray(); // Get a Span<T> from the list

            for (int i = 0; i < span.Length; i++)
            {
                if (predicate(span[i]))
                {
                    return span[i];
                }
            }
            return default;
        }
        public static T FindAsSpan<T>(this T[] array, Predicate<T> predicate)
        {
            Span<T> span = array; // Get a Span<T> from the list

            for (int i = 0; i < span.Length; i++)
            {
                if (predicate(span[i]))
                {
                    return span[i];
                }
            }
            return default;
        }
        public static Span<T> Sort<T>(this Span<T> span, Comparison<T> comparison)
        {// Convert Span to array for sorting
            var array = span.ToArray();

            // Sort the array
            Array.Sort(array, comparison);

            // Copy sorted array back into the Span
            array.CopyTo(span);
            return span;
        }

        #endregion

    }
}
