using UnityEngine;
using UnityEngine.AddressableAssets;

namespace UniGreenModules.UniGame.AddressableTools.Runtime.AssetReferencies
{
    using System;

    public class ScriptableObjectAssetReferenceT<T> : DisposableAssetReference<T>
        where T : ScriptableObject
    {
        public ScriptableObjectAssetReferenceT(string guid) : base(guid)
        {
        }

    }
}
