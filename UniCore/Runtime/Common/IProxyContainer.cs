namespace UniGreenModules.UniCore.Runtime.Common
{
    using System.Collections.Generic;
    using Interfaces;

    public interface IProxyContainer<TSource, TTarget> : IContainer<TTarget> where TSource : class, TTarget
    {
        void UpdateCollection();
        void AddRange(IReadOnlyList<TSource> sources);
        void Add(TTarget item);
        void Release();
    }
}