using UnityEngine;

namespace UniModules.UniGame.SerializableContext.Runtime.Addressables
{
    using System;
    using UnityEngine.AddressableAssets;
    using UnityEngine.U2D;

    [Serializable]
    public class AssetReferenceSpriteAtlas : AssetReferenceT<SpriteAtlas>
    {
        public AssetReferenceSpriteAtlas(string guid) : base(guid)
        {
        }
    }
}
