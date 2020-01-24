using UnityEngine;
using UnityEngine.AddressableAssets;

namespace UniGreenModules.UniGameSystems.Runtime.Addressables
{
    using System;

    [Serializable]
    public class ScriptableObjectAssetReference : AssetReferenceT<ScriptableObject>
    {
        public ScriptableObjectAssetReference(string guid) : base(guid) {}
    }
}
