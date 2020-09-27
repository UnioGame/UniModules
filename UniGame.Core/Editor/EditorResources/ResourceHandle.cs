namespace UniModules.UniGame.Core.EditorTools.Editor.EditorResources
{
    using System;
    using UniGreenModules.UniResourceSystem.Runtime.Interfaces;
    using UnityEngine;
    using Object = UnityEngine.Object;

    [Serializable]
    public class ResourceHandle : IResource
    {
        [NonSerialized]
        protected Object _loadedItem;
        
        [SerializeField]
        public string assetName;

        [SerializeField]
        public string assetPath;
        
        [SerializeField]
        public string guid;

        [SerializeField]
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.InlineEditor()]
        [Sirenix.OdinInspector.PreviewField(Sirenix.OdinInspector.ObjectFieldAlignment.Center)]
#endif
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
        
        public ResourceHandle Update(Object target)
        {
            this.asset = target;
            assetName = target.name;
            
            return OnUpdateAsset(this.asset);
        }

        public ResourceHandle Update() {
            
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

        protected virtual ResourceHandle OnUpdateAsset(Object targetAsset)
        {
            return this;
        }
        
    }
}
