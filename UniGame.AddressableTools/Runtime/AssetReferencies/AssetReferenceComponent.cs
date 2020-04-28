namespace UniModules.UniGame.AddressableTools.Runtime.AssetReferencies
{
    using UniGreenModules.UniGame.AddressableTools.Runtime.AssetReferencies;
    using UnityEngine;
#if UNITY_EDITOR
    using UnityEditor;
#endif
    
    public class AssetReferenceComponentValue<TApi> : 
        AssetReferenceComponent<Component, TApi> where TApi : class
    {
        public AssetReferenceComponentValue(string guid) : base(guid)
        {
        }
    }
    
    
    public class AssetReferenceComponent<TAsset> : 
        AssetReferenceComponent<TAsset, TAsset> where TAsset : Component
    {
        public AssetReferenceComponent(string guid) : base(guid)
        {
        }
    }

    public class AssetReferenceComponent<TAsset,TApi> : DisposableAssetReference<TAsset> 
        where TAsset : Component
        where TApi : class
    {
        public AssetReferenceComponent(string guid) : base(guid)
        {
        }
        
        public override bool ValidateAsset(string path)
        {
#if UNITY_EDITOR
            var prefab = AssetDatabase.LoadAssetAtPath(path, typeof(GameObject)) 
                as GameObject;
            return prefab?.GetComponent<TApi>() != null;
#else
            return false;
#endif
        }
    }
}