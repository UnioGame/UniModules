namespace UniGreenModules.UniGame.SerializableContext.Runtime.Addressables
{
    using System;
    using AddressableTools.Runtime.AssetReferencies;
    using UnityEngine;

#if ODIN_INSPECTOR
    using Sirenix.OdinInspector;
#endif
    
#if ODIN_INSPECTOR
    [DontApplyToListElements]
#endif
    [Serializable]
    public class AssetReferenceDisposableObject : DisposableAssetReference<ScriptableObject> 
    {
        public AssetReferenceDisposableObject(string guid) : base(guid) {}
    }
}
