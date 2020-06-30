namespace UniGreenModules.UniGame.SerializableContext.Runtime.Addressables
{
    using System;
    using AddressableTools.Runtime.AssetReferencies;
    using UnityEngine;
    using Object = UnityEngine.Object;
#if ODIN_INSPECTOR
    using Sirenix.OdinInspector;
#endif

    [Serializable]
    public class AssetReferenceScriptableObject<T> : AssetReferenceScriptableObject<ScriptableObject,T>
    {
        public AssetReferenceScriptableObject(string guid) : base(guid) {}
    }
    
    [Serializable]
    public class AssetReferenceScriptableObject<T,TApi> : DisposableAssetReference<T> 
        where T : ScriptableObject
    {
        public AssetReferenceScriptableObject(string guid) : base(guid) {}

        public override bool ValidateAsset(string path)
        {
#if UNITY_EDITOR
            var asset = UnityEditor.AssetDatabase.LoadAssetAtPath<ScriptableObject>(path);
            return asset is T && asset is TApi;
#else
            return false;
#endif
        }

        public override bool ValidateAsset(Object obj)
        {
            return obj is T && obj is TApi;
        }
    }

    [Serializable]
    public class AssetReferenceScriptableObject : DisposableAssetReference<ScriptableObject> 
    {
        public AssetReferenceScriptableObject(string guid) : base(guid) {}
    }
}
