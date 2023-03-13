namespace UniGame.LeoEcs.Debug.Editor
{
    using System;
    using System.Buffers;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Leopotam.EcsLite;
    using Runtime.ObjectPool.Extensions;

    [Serializable]
    public class FilterEntitiesComponents : IEcsWorldSearchFilter
    {
        public int[] entities = Array.Empty<int>();
        public Type[] componentsTypesOnEntity = Array.Empty<Type>();
        public HashSet<Type> componentTypes = new HashSet<Type>();
        public StringBuilder messageBuilder = new StringBuilder();
        public List<int> filteredEntities = new List<int>();

        public EcsFilterData Execute(EcsFilterData filterData)
        {
            var world = filterData.world;
            entities = Array.Empty<int>();
            
            world.GetAllEntities(ref entities);
            foreach (var entity in entities)
            {
                if(!IsContainFilteredComponent(entity,filterData))
                    continue;
                filterData.entities.Add(entity);
            }

            return filterData;
        }

        public bool IsContainFilteredComponent(
            int entity,
            EcsFilterData filterData)
        {
            var filter = filterData.filter;
            var world = filterData.world;
            var count = world.GetComponentsCount(entity);
            var found = false;
            
            componentsTypesOnEntity = ArrayPool<Type>.Shared.Rent(count);
            world.GetComponentTypes(entity,ref componentsTypesOnEntity);

            foreach (var type in componentsTypesOnEntity)
            {
                if(type == null) continue;
                if(!type.Name.Contains(filter,StringComparison.OrdinalIgnoreCase))
                    continue;
                found = true;
                break;
            }

            return found;
        }
    }
}