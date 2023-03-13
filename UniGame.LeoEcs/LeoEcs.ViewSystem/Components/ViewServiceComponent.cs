namespace UniGame.LeoEcs.ViewSystem.Components
{
    using System;
    using UniGame.ViewSystem.Runtime;

    [Serializable]
    public struct ViewServiceComponent
    {
        public IGameViewSystem ViewSystem;
    }
}