namespace UniModules.UniGame.AddressableTools.Runtime.SpriteAtlases
{
    using System;
    using Abstract;

    public interface IAddressableSpriteAtlasHandler : IAddressablesAtlasesLoader
    {
        IDisposable Execute();
        
        void Unload();
        
    }
}