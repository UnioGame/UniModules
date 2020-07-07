namespace UniModules.UniGame.AddressableTools.Runtime.SpriteAtlases
{
    using System;
    using UnityEngine.AddressableAssets;

    [Serializable]
    public class AssetReferenceAtlasHandler : AssetReferenceT<AddressableSpriteAtlasConfiguration>
    {
        public AssetReferenceAtlasHandler(string guid) : base(guid)
        {
        }
    }
}