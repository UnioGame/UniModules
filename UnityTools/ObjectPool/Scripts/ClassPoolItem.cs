using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Tools.UnityTools.ObjectPool.Scripts
{
    [Serializable]
    public class ClassPoolItem
    {
        [NonSerialized]
        private Stack<object> _items = new Stack<object>();
        [NonSerialized]
        private Type _type;
		
        [SerializeField]
        private string _typeName;
        [SerializeField]
        public int Count;

        public ClassPoolItem(Type type)
        {
            _type = type;
            _typeName = type.Name;
            Count = 0;
        }

        public void Clear()
        {
            Count = 0;
            _type = null;
            _items.Clear();
            _typeName = string.Empty;
        }
		
        public void Push(object item)
        {
            _items.Push(item);
            Count++;
        }

        public object Pop()
        {
            if (Count == 0)
                return null;
			
            Count--;
            return _items.Pop();
        }
    }
}