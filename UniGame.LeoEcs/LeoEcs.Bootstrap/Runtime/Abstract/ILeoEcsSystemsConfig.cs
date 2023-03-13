namespace UniGame.LeoEcs.Bootstrap.Runtime.Abstract
{
    using System.Collections.Generic;
    using Config;

    public interface ILeoEcsSystemsConfig
    {
        IReadOnlyList<LeoEcsConfigGroup> FeatureGroups { get; }
    }
}