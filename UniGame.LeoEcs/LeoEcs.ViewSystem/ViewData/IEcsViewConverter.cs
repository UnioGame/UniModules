namespace UniGame.LeoEcs.ViewSystem.Converters
{
    using Converter.Runtime;
    using Core.Runtime;
    using Leopotam.EcsLite;

    public interface IEcsViewConverter :
        IConverterEntityDestroyHandler
    {
        bool IsEnabled { get; }
        bool IsEntityAlive { get; }
        EcsWorld World { get; }
        EcsPackedEntity PackedEntity { get; }
        int Entity { get; }
    }
}