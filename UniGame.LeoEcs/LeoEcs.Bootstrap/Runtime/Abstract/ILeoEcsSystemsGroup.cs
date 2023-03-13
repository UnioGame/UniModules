namespace UniGame.LeoEcs.Bootstrap.Runtime.Abstract
{
    using System.Collections.Generic;
    using Leopotam.EcsLite;

    public interface ILeoEcsSystemsGroup : ILeoEcsGroupData
    {
        IReadOnlyList<IEcsSystem> EcsSystems { get; }
    }

    public interface ILeoEcsGroupData : ILeoEcsFeature
    {

    }
}