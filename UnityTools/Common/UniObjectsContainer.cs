using System;
using System.Collections.Generic;
using UniModule.UnityTools.ObjectPool.Scripts;
using UnityEngine;
using UnityEngine.Assertions;
using Object = UnityEngine.Object;

namespace UniTools.UniUiSystem
{
    [Serializable]
    public class UniObjectsContainer<TSource,TTarget> : 
        IContainer<TTarget>
        where TSource : class,TTarget
    {
        #region inspector
    
        [SerializeField]
        protected List<TSource> _items = new List<TSource>();

        #endregion

        private List<TTarget> _targetItems = new List<TTarget>();
    
        #region public properties

        public IReadOnlyList<TTarget> Items => _targetItems;
    
        #endregion
    
        public void UpdateCollection()
        {
        
            _targetItems.Clear();
            
            for (var i = 0; i < _items.Count; i++)
            {
                var item = _items[i];
                _targetItems.Add(item);
            }
        
        }

        public void Add(TTarget item)
        {
            var sourceItem = item as TSource;
            
            Assert.IsNotNull(sourceItem,$"{this.GetType()}.ADD Type Missmatch {item.GetType()} instead of {typeof(TSource)}");

            _items.Add(sourceItem);
        }
        
        public virtual void Release()
        {
            _targetItems.Clear();
            _items.Clear();
        }
    }
}
