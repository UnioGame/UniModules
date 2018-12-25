using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Tools.UnityTools.ObjectPool.Scripts
{
    public class ClassPoolContainer : MonoBehaviour
    {
        private Dictionary<Type, int> _typeIndexes = new Dictionary<Type, int>();
		
        #region inspector
		
        [SerializeField]
        private List<ClassPoolItem> _items = new List<ClassPoolItem>();
		
        #endregion

        public bool Contains(Type type)
        {
            if (!_typeIndexes.TryGetValue(type, out var index))
            {
                return false;
            }
            return _items[index].Count > 0;
        }

        public object Pop(Type type)
        {
			
            if (!_typeIndexes.TryGetValue(type, out var index))
            {
                return null;
            }

            var container = _items[index];
            var item = container.Pop();
            return item;
			
        }

        public void Push(Type type,object item)
        {
            if (_typeIndexes.TryGetValue(type, out var index))
            {
                _items[index].Push(item);
                return;
            }
            
            var container = new ClassPoolItem(type);
            index = _items.Count;
            _items.Add(container);
            
            _typeIndexes[type] = index;
        }
		
    }
}