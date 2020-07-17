namespace UniModules.UniGame.Core.EditorTools.Editor.EditorResources
{
    using System;
    using UniGreenModules.UniResourceSystem.Runtime.Interfaces;
    using UnityEngine;
    using Object = UnityEngine.Object;

    [Serializable]
    public class ResourceItem : IResource
    {
        [NonSerialized]
        protected Object _loadedItem;
        
        [HideInInspector]
        [SerializeField]
        public string assetPath;
        
        [HideInInspector]
        [SerializeField]
        public string guid;

        [HideInInspector]
        [SerializeField]
        public string assetName;
        
        [SerializeField]
        public Object asset;

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
        
        public ResourceItem Update(Object target)
        {
            this.asset = target;
            assetName = target.name;
            
            return OnUpdateAsset(this.asset);
        }

        public ResourceItem Update() {
            
            if (this.asset)
            {
                Update(this.asset);
                return this;
            }

            var target = Load<Object>();
            if(target)
            {
                Update(target);
            }

            return this;
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

        protected virtual ResourceItem OnUpdateAsset(Object targetAsset)
        {
            return this;
        }
        
    }
}
