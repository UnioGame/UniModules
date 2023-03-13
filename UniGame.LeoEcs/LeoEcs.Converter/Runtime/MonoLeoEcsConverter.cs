using Sirenix.OdinInspector;

namespace UniGame.LeoEcs.Converter.Runtime
{
    using System;
    using System.Threading;
    using Abstract;
    using Leopotam.EcsLite;
    using UnityEngine;

    [Serializable]
    [RequireComponent(typeof(LeoEcsMonoConverter))]
    public abstract class MonoLeoEcsConverter : MonoBehaviour, ILeoEcsMonoComponentConverter
    {
        [SerializeField]
        private bool _isEnabled = true;

        public bool IsPlaying => Application.isPlaying;
        
        public virtual bool IsEnabled => _isEnabled;
        
        public abstract void Apply(GameObject target, EcsWorld world, int entity, CancellationToken cancellationToken = default);

    }
}