namespace UniGame.LeoEcs.Converter.Runtime.Converters
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using Abstract;
    using Cysharp.Threading.Tasks;
    using Leopotam.EcsLite;
    using Sirenix.OdinInspector;
    using UnityEngine;

    public class MonoLeoEcsGroupConverter : MonoLeoEcsConverter<LeoEcsGroupConverter>
    {
    }

    [Serializable]
    public class LeoEcsGroupConverter : LeoEcsConverter
    {
        [SerializeField]
        private string groupName;
        
        [InlineProperty]
        [SerializeReference]
        private List<ILeoEcsMonoComponentConverter> _converters = new List<ILeoEcsMonoComponentConverter>();

        public override void Apply(GameObject target, EcsWorld world, int entity, CancellationToken cancellationToken = default)
        {
            foreach (var converter  in _converters)
                converter.Apply(target, world, entity, cancellationToken);
        }
    }
}