namespace UniGreenModules.UniCore.Runtime.Common
{
    using System;
    using System.Collections.Generic;
    using Interfaces;
    using UnityEngine;

    [Serializable]
    public class UniObjectsContainer<TSource,TTarget> : 
        IProxyContainer<TSource, TTarget> 
        where TSource : class,TTarget
    {
        #region inspector
    
        [SerializeField]
        protected List<TSource> _items = new List<TSource>();

        #endregion

        #region public properties

        public IReadOnlyList<TTarget> Items => _items;
    
        #endregion
        
        public void AddRange(IReadOnlyList<TSource> sources)
        {
            for (var i = 0; i < sources.Count; i++) {
                var item = sources[i];
                Add(item);
            }
        }
        
        public void Add(TTarget item)
        {
            if (!(item is TSource sourceItem))
            {
                Debug.LogError($"{this.GetType()}.ADD Type Missmatch {item.GetType()} instead of {typeof(TSource)}");
                return;
            }

            _items.Add(sourceItem);
            
            OnSourceItemAdded(sourceItem);
        }
        
        public void Release()
        {
            _items.Clear();
            OnRelease();
        }


        protected virtual void OnSourceItemAdded(TSource source){}

        protected virtual void OnRelease(){}
        
    }
}
