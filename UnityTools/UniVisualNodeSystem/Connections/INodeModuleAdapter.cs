using System;
using System.Collections.Generic;
using Assets.Tools.UnityTools.Interfaces;
using UnityTools.UniVisualNodeSystem;

namespace UnityTools.UniNodeEditor.Connections
{
    public interface INodeModuleAdapter
    {
        IReadOnlyCollection<PortDefinition> Ports { get; }

        IDisposable Bind(string portName,IContextData<IContext> portValue,IContext context);
        
        void Execute(string portName,IContextData<IContext> portValue, IContext context);
    }
}