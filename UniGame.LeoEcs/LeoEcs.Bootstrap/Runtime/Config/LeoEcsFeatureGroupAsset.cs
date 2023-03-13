using System;
using Cysharp.Threading.Tasks;
using UniGame.Core.Runtime.ScriptableObjects;

namespace UniGame.LeoEcs.Bootstrap.Runtime.Config
{
    using System.Collections.Generic;
    using Abstract;
    using Leopotam.EcsLite;
    using Sirenix.OdinInspector;
    using UnityEngine;

    [CreateAssetMenu(menuName = "UniGame/LeoEcs/Feature/ECS Feature", fileName = "ECS Feature")]
    public class LeoEcsFeatureGroupAsset : 
        BaseLeoEcsFeature, 
        ILeoEcsSystemsGroup
    {
        #region inspector

        [HideLabel]
        [InlineProperty]
        public LeoEcsSystemsGroupConfiguration groupConfiguration = new LeoEcsSystemsGroupConfiguration();

        #endregion

        #region public properties

        public IReadOnlyList<IEcsSystem> EcsSystems => groupConfiguration.EcsSystems;
 
        public override string FeatureName => groupConfiguration.FeatureName;

        public override bool IsFeatureEnabled => groupConfiguration.IsFeatureEnabled;

        public override bool ShowFeatureInfo => false;


        #endregion
        
        public sealed override async UniTask InitializeFeatureAsync(EcsSystems ecsSystems)
        {
            await OnInitializeFeatureAsync(ecsSystems);
            await groupConfiguration.InitializeFeatureAsync(ecsSystems);
            await OnPostInitializeFeatureAsync(ecsSystems);
        }

        public override bool IsMatch(string searchString)
        {
            if (base.IsMatch(searchString)) return true;

            return groupConfiguration.IsMatch(searchString);
        }
        
        protected virtual UniTask OnInitializeFeatureAsync(EcsSystems ecsSystems)
        {
            return UniTask.CompletedTask;
        }
        
        protected virtual UniTask OnPostInitializeFeatureAsync(EcsSystems ecsSystems)
        {
            return UniTask.CompletedTask;
        }
        
        [OnInspectorInit]
        private void OnInspectorInitialize()
        {
            if (groupConfiguration != null && 
                string.IsNullOrEmpty(groupConfiguration.FeatureName))
                groupConfiguration._name = name;
        }
    }
}