namespace UniGame.LeoEcs.Bootstrap.Runtime.Abstract
{
    using System;
    using Leopotam.EcsLite;

    public interface ILeoEcsExecutor : IDisposable
    {
        bool IsActive { get; }

        void Execute(EcsWorld world);

        void Add(EcsSystems systems);

        void Stop();
    }
    
    public interface ISystemsPlugin : IDisposable
    {
        bool IsActive { get; }

        void Execute(EcsWorld world);

        void Add(EcsSystems systems);

        void Stop();
    }
}