using UnityEngine;

namespace UniGreenModules.UniGame.AddressableTools.Runtime.AssetReferencies
{
    using System;

    [Serializable]
    public class ScriptableObjectAssetReferenceT<T> : DisposableAssetReference<T>
        where T : ScriptableObject
    {
        public ScriptableObjectAssetReferenceT(string guid) : base(guid)
        {
        }
    }
}
