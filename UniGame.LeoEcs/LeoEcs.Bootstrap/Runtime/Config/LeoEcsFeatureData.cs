namespace UniGame.LeoEcs.Bootstrap.Runtime.Config
{
    using System;
    using Cysharp.Threading.Tasks;
    using System.Collections.Generic;
    using Leopotam.EcsLite;
    using Sirenix.OdinInspector;
    using Abstract;
    using UnityEngine;

    [Serializable]
    public class LeoEcsFeatureData : ILeoEcsGroupData,ISearchFilterable
    {
        private static List<IEcsSystem> emptySystems = new List<IEcsSystem>();
        
        public const string EcsEmptyFeatureName = "Empty Feature";

        [FoldoutGroup("$"+nameof(FeatureName))]
        [InlineEditor]
        [HideLabel]
        [ShowIf(nameof(UseAssetGroup))]
        public LeoEcsFeatureGroupAsset featureGroupAsset;

        [FoldoutGroup("$"+nameof(FeatureName))]
        [SerializeReference]
        [HideLabel]
        [ShowIf(nameof(UseSerializedGroup))]
        public ILeoEcsSystemsGroup featureGroup;

        public string FeatureName => Feature == null 
            ? EcsEmptyFeatureName 
            : Feature.FeatureName;

        public bool UseSerializedGroup => featureGroup != null || 
                                          (featureGroupAsset == null && featureGroup == null);
        
        public bool UseAssetGroup => featureGroupAsset != null || 
                                     (featureGroupAsset == null && featureGroup == null);

        public ILeoEcsSystemsGroup Feature => UseSerializedGroup 
            ? featureGroup 
            : featureGroupAsset;

        public bool IsFeatureEnabled => Feature is { IsFeatureEnabled: true };

        public IReadOnlyList<IEcsSystem> EcsSystems => Feature == null 
            ? emptySystems 
            : Feature.EcsSystems;
        
        public bool IsMatch(string searchString)
        {
            if (string.IsNullOrEmpty(searchString)) return true;

            if (UseSerializedGroup && nameof(UseSerializedGroup).Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;

            if (Feature != null && 
                Feature.GetType().Name
                    .Contains(searchString, StringComparison.OrdinalIgnoreCase)) 
                return true;

            if (EcsSystems == null)
                return false;
            
            foreach (var system in EcsSystems)
            {
                if (system == null) continue;
                    
                if (system is ISearchFilterable searchFilterable && searchFilterable.IsMatch(searchString))
                    return true;
                
                if (system.GetType()
                    .Name
                    .Contains(searchString, StringComparison.OrdinalIgnoreCase)) 
                    return true;
            }

            return false;
        }

        public UniTask InitializeFeatureAsync(EcsSystems ecsSystems)
        {
            if(Feature == null) return UniTask.CompletedTask;
            return Feature.InitializeFeatureAsync(ecsSystems);
        }
    }
}
