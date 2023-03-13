using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace UniGame.LeoEcs.Bootstrap.Runtime
{
    using Cysharp.Threading.Tasks;
    using Leopotam.EcsLite;
    using UniGame.Core.Runtime.ScriptableObjects;
    using Abstract;
    
    public abstract class BaseLeoEcsFeature : LifetimeScriptableObject,ILeoEcsFeature
    {
        [ShowIf(nameof(ShowFeatureInfo))]
        [SerializeField]
        public bool isEnabled = true;

        public virtual bool IsFeatureEnabled => isEnabled;

        public virtual string FeatureName => name;

        public virtual bool ShowFeatureInfo => true;

        public abstract UniTask InitializeFeatureAsync(EcsSystems ecsSystems);

        public virtual bool IsMatch(string searchString)
        {
            if (string.IsNullOrEmpty(searchString)) return true;
            
            if (ContainsSearchString(name,searchString)) 
                return true;
            
            return ContainsSearchString(FeatureName, searchString);
        }

        protected bool ContainsSearchString(string source, string filter)
        {
            return !string.IsNullOrEmpty(source) && 
                   source.Contains(filter, StringComparison.OrdinalIgnoreCase);
        }

    }
}