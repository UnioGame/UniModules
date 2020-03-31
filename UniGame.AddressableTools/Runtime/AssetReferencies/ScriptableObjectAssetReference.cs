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
    public class AssetReferenceScriptableObject : DisposableAssetReference<ScriptableObject> 
    {
        public AssetReferenceScriptableObject(string guid) : base(guid) {}
    }
}
