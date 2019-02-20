using System.Collections.Generic;

namespace UniTools.UniUiSystem
{
    public interface IContainer<out TData>
    {
        IReadOnlyList<TData> Items { get; }
    }
}