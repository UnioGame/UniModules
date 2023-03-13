namespace UniGame.LeoEcs.Bootstrap.Runtime.Abstract
{
    using Leopotam.EcsLite;

    public interface ILeoEcsSystem : IEcsSystem
    {
        bool Enabled { get; }
    }
}