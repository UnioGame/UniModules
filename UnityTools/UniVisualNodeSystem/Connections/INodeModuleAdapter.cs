using System.Collections.Generic;
using Assets.Modules.UnityToolsModule.Tools.UnityTools.DataFlow;
using Assets.Tools.UnityTools.Interfaces;
using UnityEngine;
using UnityTools.UniVisualNodeSystem;

namespace UnityTools.UniNodeEditor.Connections
{
    public interface INodeModuleAdapter
    {
        IReadOnlyCollection<PortDefinition> Ports { get; }

        void Bind(IContext context, ILifeTime lifeTime);
        
        void Execute( IContext context, ILifeTime lifeTime);
    }
}