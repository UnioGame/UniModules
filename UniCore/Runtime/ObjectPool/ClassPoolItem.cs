namespace UniGreenModules.UniCore.Runtime.ObjectPool
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    [Serializable]
    public class ClassPoolItem
    {
        [NonSerialized]
        private Stack<object> _items = new Stack<object>();
        
        [NonSerialized]
        public Type type;
		[SerializeField]
        public string typeName;
        [SerializeField]
        public int count;

        public ClassPoolItem(Type type)
        {
            this.type = type;
            typeName = type.Name;
            count = 0;
        }

        public void Clear()
        {
            count = 0;
            type = null;
            _items.Clear();
            typeName = string.Empty;
        }
		
        public void Push(object item)
        {
            _items.Push(item);
            count++;
        }

        public object Pop()
        {
            if (count == 0)
                return null;
			
            count--;
            return _items.Pop();
        }
    }
}