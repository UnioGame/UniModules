namespace UniGreenModules.UniCore.EditorTools.Editor.AssetOperations
{
    using System;
    using Runtime.ObjectPool.Runtime.Interfaces;
    using UnityEditor;
    using Object = UnityEngine.Object;

    [Serializable]
    public class EditorAssetResource : IPoolable, IEditorAssetResource
    {
        private string path;
        private Object asset;

        public IEditorAssetResource Initialize(string assetPath)
        {
            path = assetPath;
            return this;
        }
        
        public IEditorAssetResource Initialize(Object sourceAsset)
        {
            asset = sourceAsset;
            path = AssetDatabase.GetAssetPath(sourceAsset);
            return this;
        }

        public bool HasData<T>()
            where  T : class
        {
            var resource = Load<T>();
            return resource != null;
        }
        
        public T Load<T>()
            where  T : class
        {
            if (string.IsNullOrEmpty(path)) {
                return null;
            }
            
            if (!asset) {
                asset = AssetDatabase.LoadAssetAtPath<Object>(path);
            }

            return asset as T;
        }

        public void Release()
        {
            path = null;
            asset = null;
        }
    }
}
