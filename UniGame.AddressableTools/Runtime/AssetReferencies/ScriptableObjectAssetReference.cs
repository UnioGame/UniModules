namespace UniGreenModules.UniGame.SerializableContext.Runtime.Addressables
{
    using System;
    using AddressableTools.Runtime.AssetReferencies;
    using UnityEngine;
    using UnityEngine.AddressableAssets;

#if ODIN_INSPECTOR
    using Sirenix.OdinInspector;
#endif
    
#if ODIN_INSPECTOR
    [DontApplyToListElements]
#endif
    
    [Serializable]
    public class ScriptableObjectAssetReference : DisposableAssetReference<ScriptableObject> 
    {
        public ScriptableObjectAssetReference(string guid) : base(guid) {}
    }
}
