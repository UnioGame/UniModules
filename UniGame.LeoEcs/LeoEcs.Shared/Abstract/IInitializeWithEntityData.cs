namespace UniGame.LeoEcs.Shared.Abstract
{
    using Leopotam.EcsLite;

    public interface IInitializeWithEntityData
    {
        public void InitializeEcsData(EcsWorld world, int entity);
    }
}
