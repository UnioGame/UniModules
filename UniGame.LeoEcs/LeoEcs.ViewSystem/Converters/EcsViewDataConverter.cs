namespace UniGame.LeoEcs.ViewSystem.Converters
{
    using System;
    using System.Threading;
    using Components;
    using Converter.Runtime.Abstract;
    using Leopotam.EcsLite;
    using Shared.Extensions;
    using UnityEngine;

    [Serializable]
    public class EcsViewDataConverter<TData> : ILeoEcsMonoComponentConverter
    {
        public bool addUpdateRequestOnCreate = true;
        public bool IsEnabled => true;
        
        public void Apply(GameObject target, EcsWorld world, int entity, CancellationToken cancellationToken = default)
        {
            ref var dataComponent = ref world.GetOrAddComponent<ViewDataComponent<TData>>(entity);
            if (addUpdateRequestOnCreate)
                world.AddComponent<UpdateViewRequest<TData>>(entity);
        }
    }
}