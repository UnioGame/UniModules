namespace UniGreenModules.UniGame.SerializableContext.Runtime.Addressables
{
    using System;
    using UnityEngine;
    using UnityEngine.AddressableAssets;

    [Serializable]
    public class ScriptableObjectAssetReference : AssetReferenceT<ScriptableObject>
    {
        public ScriptableObjectAssetReference(string guid) : base(guid) {}
    }
}
