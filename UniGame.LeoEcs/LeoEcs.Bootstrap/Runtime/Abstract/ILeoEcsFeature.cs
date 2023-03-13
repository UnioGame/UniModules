namespace UniGame.LeoEcs.Bootstrap.Runtime.Abstract
{
    using Sirenix.OdinInspector;

    public interface ILeoEcsFeature : ILeoEcsInitializableFeature, ISearchFilterable
    {
        bool IsFeatureEnabled { get; }
        string FeatureName {get;}
    }
}