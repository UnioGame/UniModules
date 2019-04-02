
using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UniModule.UnityTools.ResourceSystem
{
    [Serializable]
    public class ResourceItem : IResource
    {
        [NonSerialized]
        protected Object _loadedItem;
        
        [HideInInspector]
        [SerializeField]
        protected string assetPath;
        
        [HideInInspector]
        [SerializeField]
        protected string guid;

        [HideInInspector]
        [SerializeField]
        protected string assetName;
        
        [SerializeField]
        protected Object asset;

        public string ItemName => assetName;
        
        #region public methods

        public bool HasValue()
        {
            return asset;
        }
        
        public T Load<T>()
            where T : Object
        {
            T result = null;
            if (_loadedItem != null)
            {
                
                if (_loadedItem is T cached)
                    return cached;

                result = GetTargetFromSource<T>(_loadedItem);

            }
            else if (asset)
            {
                result = GetTargetFromSource<T>(asset);
            }
            else
            {
                result = LoadAsset<T>();
            }

            ApplyResource(result);

            return result;
        }
        
        public void Update(Object target)
        {
            this.asset = target;
            assetName = target.name;
            
            OnUpdateAsset(this.asset);
        }

        public void Update() {
            
            if (this.asset)
            {
                Update(this.asset);
                return;
            }

            var target = Load<Object>();
            if(target)
            {
                Update(target);
            }
            
        }

        #endregion

        protected T GetTargetFromSource<T>(Object source)
            where  T : Object
        {
                        
            var result = source as T;
            if (result)
            {
                return result;
            }

            if (source is GameObject gameObject)
            {
                result = gameObject.GetComponent<T>();
                if (result)
                {
                    return result;
                }
            }

            return result;
            
        }
        
        private T ApplyResource<T>(T resource)
            where T : Object
        {
            _loadedItem = resource;
            return resource;
        }

        protected virtual TResult LoadAsset<TResult>()
            where TResult : Object
        {
            return null;
        }

        protected virtual void OnUpdateAsset(Object targetAsset)
        {
            
        }
        
    }
}
