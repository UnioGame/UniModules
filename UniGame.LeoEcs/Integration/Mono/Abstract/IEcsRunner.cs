namespace UniGame.ECS.Mono.Abstract
{
    using System;
    using Cysharp.Threading.Tasks;
    using Leopotam.EcsLite;
    using UniModules.UniGame.Core.Runtime.Interfaces;

    public interface IEcsRunner : IDisposable, ILifeTimeContext
    {
        UniTask Execute(IContext context,EcsWorld world);
    }
}