namespace UniModules.UniGame.AddressableTools.Runtime.SpriteAtlases
{
    using System;

    public interface IAddressableSpriteAtlasHandler 
    {
        IDisposable Execute();
        void Unload();
    }
}