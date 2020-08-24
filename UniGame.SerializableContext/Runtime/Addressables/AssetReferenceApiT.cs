namespace UniModules.UniGame.SerializableContext.Runtime.Addressables
{
    using System;
    using UnityEngine;
    using UnityEngine.AddressableAssets;
    using Object = UnityEngine.Object;

    [Serializable]
    public class AssetReferenceApiT<TApi> : AssetReferenceT<Object>
    {
        public AssetReferenceApiT(string guid) : base(guid)
        {
        }

        public override bool ValidateAsset(Object obj)
        {
            switch (obj) {
                case TApi value:
                case GameObject gameObject when gameObject.GetComponent<TApi>() != null:
                    return true;
            }
            return false;
        }
    }
}