namespace UniGame.LeoEcs.Converter.Runtime.Extensions
{
    using Core.Runtime;
    using Shared.Extensions;

    public static class EntityExtensions
    {
                
        public static ILifeTime DestroyEntityWith(this ILifeTime lifeTime, int entity)
        {
            var world = LeoEcsConvertersData.World;
            return lifeTime.DestroyEntityWith(entity, world);
        }
    }
}