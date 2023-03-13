namespace UniGame.LeoEcs.Bootstrap.Runtime
{
    using System;
    using Abstract;
    using Cysharp.Threading.Tasks;
    using Leopotam.EcsLite;

    [Serializable]
    public abstract class LeoEcsSystemAsyncFeature : IEcsSystem, ILeoEcsFeature
    {
        public bool enabled = true;
        public string featureName = string.Empty;

        public bool IsFeatureEnabled => enabled;

        public string FeatureName => string.IsNullOrEmpty(featureName)
            ? this.GetType().Name
            : featureName;
        
        public abstract UniTask InitializeFeatureAsync(EcsSystems ecsSystems);

        public virtual bool IsMatch(string searchString)
        {
            if (string.IsNullOrEmpty(searchString)) return true;

            var typeName = GetType().Name;
            if (typeName.IndexOf(searchString, StringComparison.OrdinalIgnoreCase) >= 0) 
                return true;
            return false;
        }

    }
}