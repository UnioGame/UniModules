namespace UniModules.UniGame.AddressableTools.Runtime.SpriteAtlases
{
    using System;
    using System.Collections.Generic;

    public interface IAddressableSpriteAtlasHandler 
    {
        IDisposable Execute();
        void Unload();
        
    }
}