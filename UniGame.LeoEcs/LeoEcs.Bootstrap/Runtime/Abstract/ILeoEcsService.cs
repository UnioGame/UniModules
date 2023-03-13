namespace UniGame.LeoEcs.Bootstrap.Runtime.Abstract
{
    using UniGame.GameFlow.Runtime.Interfaces;
    using System;
    using Converter.Runtime;
    using Leopotam.EcsLite;

    public interface ILeoEcsService : IGameService
    {

        EcsWorld World { get; }
        
        public void SetDefaultWorld(EcsWorld world);

    }
}