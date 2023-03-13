using System;
using System.Threading;
using Leopotam.EcsLite;
using Sirenix.OdinInspector;
using UniGame.LeoEcs.Converter.Runtime.Abstract;
using UnityEngine;

namespace UniGame.LeoEcs.Converter.Runtime
{
    [CreateAssetMenu(menuName = "UniGame/LeoEcs/Converter/GameObject Converter",fileName = "GameObject Converter")]
    public class GameObjectAssetConverter : ScriptableObject,IComponentConverter,ILeoEcsMonoComponentConverter
    {
        [HideLabel]
        [InlineProperty]
        public GameObjectConverter converter = new GameObjectConverter();
        
        public bool IsEnabled => converter.IsEnabled;
        
        public void Apply(EcsWorld world, int entity, CancellationToken cancellationToken = default)
        {
            converter.Apply(world,entity,cancellationToken);
        }

        public void Apply(GameObject target, EcsWorld world, int entity, CancellationToken cancellationToken = default)
        {
            converter.Apply(target,world,entity,cancellationToken);
        }

        public bool IsMatch(string searchString)
        {
            if (converter.IsMatch(searchString)) return true;
            if (name.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            return false;
        }
    }
}