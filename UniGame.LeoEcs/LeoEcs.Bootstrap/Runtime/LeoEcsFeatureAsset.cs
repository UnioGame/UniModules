namespace UniGame.LeoEcs.Bootstrap.Runtime
{
    using System;
    using System.Collections.Generic;
    using Abstract;
    using Cysharp.Threading.Tasks;
    using Leopotam.EcsLite;
    using UnityEngine;

#if ODIN_INSPECTOR
    using Sirenix.OdinInspector;
#endif
    
    public class LeoEcsFeatureAsset : BaseLeoEcsFeature , ILeoEcsFeature
    {
        #region inspector

#if ODIN_INSPECTOR
        [InlineEditor()]
#endif
        public List<LeoEcsFeatureAsset> subFeatures = new List<LeoEcsFeatureAsset>();

        [SerializeReference]
        public List<ILeoEcsFeature> serializableFeatures = new List<ILeoEcsFeature>();

        #endregion

        public sealed override async UniTask InitializeFeatureAsync(EcsSystems ecsSystems)
        {
            if (!IsFeatureEnabled) return;
            
            foreach (var featureAsset in subFeatures)
                await featureAsset.InitializeFeatureAsync(ecsSystems);

            foreach (var ecsFeature in serializableFeatures)
                await ecsFeature.InitializeFeatureAsync(ecsSystems);

            await OnInitializeFeatureAsync(ecsSystems);
        }
        
        public override bool IsMatch(string searchString)
        {
            if (string.IsNullOrEmpty(searchString)) return true;

            var typeName = GetType().Name;
            if (typeName.IndexOf(searchString, StringComparison.OrdinalIgnoreCase) >= 0) 
                return true;
            if (name.IndexOf(searchString, StringComparison.OrdinalIgnoreCase) >= 0) 
                return true;
            if (FeatureName.IndexOf(searchString, StringComparison.OrdinalIgnoreCase) >= 0)
                return true;

            foreach (var featureAsset in subFeatures)
            {
                if (featureAsset.IsMatch(searchString))
                    return true;
            }

            foreach (var featureAsset in serializableFeatures)
            {
                if (featureAsset.IsMatch(searchString))
                    return true;
            }
            
            return false;
        }

        protected virtual UniTask OnInitializeFeatureAsync(EcsSystems ecsSystems)
        {
            return UniTask.CompletedTask;
        }
    }

    public class LeoEcsFeatureAssetT<TFeature> : LeoEcsFeatureAsset
        where TFeature : ILeoEcsFeature
    {
        [HideLabel]
        [SerializeField] 
        public TFeature feature;

        protected override async UniTask OnInitializeFeatureAsync(EcsSystems ecsSystems)
        {
            await feature.InitializeFeatureAsync(ecsSystems);

        }
    }
}