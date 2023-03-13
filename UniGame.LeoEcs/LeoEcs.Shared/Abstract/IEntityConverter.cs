namespace UniGame.LeoEcs.Shared.Abstract
{
    using Leopotam.EcsLite;

    public interface IEntityConverter
    {
        void Apply(EcsWorld world, int entity);
    }
}