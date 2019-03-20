using System.Collections.Generic;
using UniModule.UnityTools.ObjectPool.Scripts;

namespace UniTools.UniUiSystem
{
    public interface IContainer<TData> : IPoolable
    {
        IReadOnlyList<TData> Items { get; }
    }
}