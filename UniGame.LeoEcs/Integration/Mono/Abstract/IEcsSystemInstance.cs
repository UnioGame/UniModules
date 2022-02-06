namespace UniGame.ECS.Mono.Abstract
{
    using Cysharp.Threading.Tasks;
    using Leopotam.EcsLite;
    using UniModules.UniGame.Core.Runtime.Interfaces;

    public interface IEcsSystemInstance
    {
        UniTask Register(IContext context,EcsWorld world, EcsSystems systems);
    }
}