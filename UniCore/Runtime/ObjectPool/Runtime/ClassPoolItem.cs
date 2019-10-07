namespace UniGreenModules.UniCore.Runtime.ObjectPool
{
    using System;
    using System.Collections.Generic;
    using Interfaces;
    using UnityEngine;

    [Serializable]
    public class ClassPoolItem<T> : BasePoolItem
        where T : class
    {
        private static ClassPoolItem<T> instance;
        
        [NonSerialized]
        private Stack<T> _items = new Stack<T>();

        public static ClassPoolItem<T> Instance {
            get
            {
                if(instance == null) 
                {
                    instance = new ClassPoolItem<T>();
                }
                return instance;
            }
        }

        protected ClassPoolItem()
        {
            typeName = typeof(T).Name;
        }

        public int Count => count;
        
        public override void Release()
        {
            count = 0;
            _items.Clear();
        }
		
        public void Push(T item)
        {
            _items.Push(item);
            count++;
        }

        public T Pop()
        {
            if (count == 0) return null;
            var item = _items.Pop();
            count--;
            return item;
        }
    }
}