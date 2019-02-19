using System.Collections.Generic;
using UnityEngine;

namespace UniTools.UniUiSystem
{
    public class AdapterContainer<TSource,TTarget> : MonoBehaviour, IContainer<TTarget>
        where TSource : Object , TTarget
    {
        #region inspector
    
        [SerializeField]
        protected List<TSource> _items = new List<TSource>();

        #endregion

        private List<TTarget> _targetItems = new List<TTarget>();
    
        #region public properties

        public List<TTarget> Items => _targetItems;
    
        #endregion
    
        public virtual void UpdateCollection()
        {
        
            _targetItems.Clear();
            _targetItems.AddRange(_items);
        
        }
    
    }
}
