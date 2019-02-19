using System.Collections.Generic;
using UniStateMachine.NodeEditor.UiNodes;

namespace UniTools.UniUiSystem
{
    public interface IContainer<TData>
    {
        List<TData> Items { get; }
    }
}