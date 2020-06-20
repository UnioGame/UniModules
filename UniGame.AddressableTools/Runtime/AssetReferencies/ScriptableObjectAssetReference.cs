namespace UniGreenModules.UniGame.SerializableContext.Runtime.Addressables
{
    using System;
    using AddressableTools.Runtime.AssetReferencies;
    using UnityEngine;

#if ODIN_INSPECTOR
    using Sirenix.OdinInspector;
#endif

    [Serializable]
    public class AssetReferenceScriptableObject<T> : DisposableAssetReference<T> 
        where T : ScriptableObject
    {
        public AssetReferenceScriptableObject(string guid) : base(guid) {}
    }

    [Serializable]
    public class AssetReferenceScriptableObject : DisposableAssetReference<ScriptableObject> 
    {
        public AssetReferenceScriptableObject(string guid) : base(guid) {}
    }
}
