
using System;
using UnityEngine;
using UniUiSystem;
using Object = UnityEngine.Object;

namespace UniModule.UnityTools.ResourceSystem
{
    [Serializable]
    public class ResourceItem : INamedItem
    {
        [NonSerialized]
        private Object _loadedItem;
        
        [HideInInspector]
        [SerializeField]
        private string assetPath;
        
        [HideInInspector]
        [SerializeField]
        private string guid;

        [SerializeField]
        private Object asset;


        public string ItemName
        {
            get { return asset ? asset.name : string.Empty; }
        }

        #region public methods

        public bool HasValue()
        {
            return asset;
        }
        
        public T Load<T>()
            where T : Object
        {
            if (_loadedItem is T cached)
                return cached;
            
            var result = asset as T;
            if (result)
            {
                return ApplyResource(result);
            }

            if (asset is GameObject gameObject)
            {
                result = gameObject.GetComponent<T>();
                if (result)
                {
                    return ApplyResource(result);
                }
            }

            Debug.LogError("LOAD NULL ASSET FROM ResourceItem");
            
            return null;
        }
        
        public void Update(Object asset)
        {
            this.asset = asset;
        }

        public void Update() {

        }

        #endregion

        private T ApplyResource<T>(T resource)
            where T : Object
        {
            _loadedItem = resource;
            return resource;
        }

        
    }
}
