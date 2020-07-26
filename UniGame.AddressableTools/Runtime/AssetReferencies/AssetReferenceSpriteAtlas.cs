namespace UniModules.UniGame.AddressableTools.Runtime.AssetReferencies
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
