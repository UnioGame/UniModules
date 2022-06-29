namespace UniGame.Utils.Runtime
{
    using System;
#if ODIN_INSPECTOR
    using Sirenix.OdinInspector;
#endif
    using UnityEngine;
    using Object = UnityEngine.Object;

    [Serializable]
#if ODIN_INSPECTOR
    [DrawWithUnity]
#endif
    public struct NullObject<T> : IEquatable<T>, IEquatable<NullObject<T>>
    {
        // ReSharper disable once Unity.RedundantSerializeFieldAttribute
        [SerializeField]
        private T _item;

        [SerializeField, HideInInspector]
        private int _nullHashCode;

        public T Item => _item;

        public bool IsNull
        {
            get
            {
                if (_item is Object unityObject)
                    return !unityObject;

                return _item == null;
            }
        }

        public NullObject(T item) : this()
        {
            _item         = item;
            _nullHashCode = Guid.NewGuid().GetHashCode();
        }

        public static NullObject<T> Null()
        {
            return new NullObject<T>(default);
        }

        public static implicit operator T(NullObject<T> nullObject)
        {
            return nullObject.Item;
        }

        public static implicit operator NullObject<T>(T item)
        {
            return new NullObject<T>(item);
        }

        public override string ToString()
        {
            return IsNull ? "Null" : Item.ToString();
        }

        public bool Equals(T other)
        {
            if (IsNull)
                return other == null;

            return Item.Equals(other);
        }

        public bool Equals(NullObject<T> other)
        {
            if (IsNull)
                return other.IsNull;
            
            return Item.Equals(other.Item);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return IsNull;

            if (!(obj is NullObject<T>))
                return false;

            var nullObject = (NullObject<T>) obj;
            if (IsNull)
                return nullObject.IsNull;
            
            if (nullObject.IsNull)
                return false;

            return Item.Equals(nullObject.Item);
        }

        public override int GetHashCode()
        {
            if (IsNull)
            {
                return _nullHashCode;
            }

            var result = Item.GetHashCode();
            if (result >= 0)
                result++;

            return result;
        }
    }
}