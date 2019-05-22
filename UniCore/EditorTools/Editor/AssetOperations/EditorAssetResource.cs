using UnityEngine;

namespace UniGreenModules.UniCore.EditorTools.AssetOperations
{
    using Runtime.ObjectPool.Interfaces;
    using UnityEditor;

    public class EditorAssetResource : IPoolable
    {
        private string path;
        private Object asset;

        public void Initialize(string assetPath)
        {
            path = assetPath;
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
