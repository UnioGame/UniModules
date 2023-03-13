namespace UniGame.LeoEcs.Converter.Runtime
{
    using System;
    using System.Collections.Generic;
    using Abstract;
    using Core.Runtime;
    using Core.Runtime.ScriptableObjects;
    using Cysharp.Threading.Tasks;
    using Leopotam.EcsLite;
    using Shared.Extensions;
    using Sirenix.OdinInspector;
    using UnityEngine;

    [Serializable]
    [CreateAssetMenu(menuName = "UniGame/LeoEcs/Converter/Entity Converter",fileName = "Entity Converter")]
    public class EcsEntityConverter : LifetimeScriptableObject
    {
        #region inspector
        
        [Tooltip("if true, when create entities for each converter")]
        public bool createEntityForEachConverter = true;

        [InlineProperty]
        public List<ComponentConverterValue> converters = new List<ComponentConverterValue>();
        
        #endregion

        public async UniTask Create(EcsWorld world,ILifeTime lifeTime)
        {
            if (!createEntityForEachConverter)
            {
                var entity = world.NewEntity();
                lifeTime.DestroyEntityWith(entity, world);
                
                await Create(entity, world);
                return;
            }
            
            foreach (var converter in converters)
            {
                var entity = world.NewEntity();
                lifeTime.DestroyEntityWith(entity, world);
                
                Convert(entity,world,converter).Forget();
            }
        }

        public async UniTask Create(int entity, EcsWorld world)
        {
            await UniTask.WhenAll(converters.Select(x => Convert(entity, world, x)));
        }

        private UniTask Convert(int entity, EcsWorld world, IComponentConverter converter)
        {
            if (!converter.IsEnabled)
                return UniTask.CompletedTask;
            
            converter.Apply(world,entity,LifeTime.TokenSource);
            
            return UniTask.CompletedTask;
        }
    }
}