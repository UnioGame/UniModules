namespace UniModules.UniGame.AddressableTools.Runtime.SpriteAtlases
{
    using System;
    using System.Collections.Generic;
    using SerializableContext.Runtime.Addressables;

    public interface IAddressableSpriteAtlasHandler 
    {
        IDisposable Execute();
        void Set(IReadOnlyList<AssetReferenceSpriteAtlas> atlases);
        void Unload();
        
    }
}