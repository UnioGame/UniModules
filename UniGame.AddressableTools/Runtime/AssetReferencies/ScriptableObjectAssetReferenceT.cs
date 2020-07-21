using UnityEngine;

namespace UniGreenModules.UniGame.AddressableTools.Runtime.AssetReferencies
{
    using System;

#if ODIN_INSPECTOR
    [Sirenix.OdinInspector.DontApplyToListElements]
#endif
    [Serializable]
    public class ScriptableObjectAssetReferenceT<T> : DisposableAssetReference<T>
        where T : ScriptableObject
    {
        public ScriptableObjectAssetReferenceT(string guid) : base(guid)
        {
        }
    }
}
