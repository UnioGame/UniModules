namespace UniModules.UniGame.AddressableTools.Runtime.AssetReferencies
{
    using UnityEngine;
    using UnityEngine.AddressableAssets;
#if UNITY_EDITOR
    using UnityEditor;
#endif

    public class AssetReferenceComponent<T> : AssetReferenceT<T> where T : Component
    {
        public AssetReferenceComponent(string guid) : base(guid)
        {
        }
        
        public override bool ValidateAsset(string path)
        {
#if UNITY_EDITOR
            var prefab = AssetDatabase.LoadAssetAtPath(path, typeof(T));
            return prefab != null;
#else
            return false;
#endif
        }
    }
}