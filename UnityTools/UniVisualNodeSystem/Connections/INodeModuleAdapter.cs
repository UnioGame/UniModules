using System.Collections.Generic;
using Assets.Modules.UnityToolsModule.Tools.UnityTools.DataFlow;
using Assets.Tools.UnityTools.Interfaces;
using UnityEngine;

namespace UnityTools.UniNodeEditor.Connections
{
    public interface INodeModuleAdapter
    {
        IReadOnlyCollection<string> Ports { get; }
        
        void BindValue(string key,IContextData<IContext> value);
        
        void Bind(IContext context, ILifeTime lifeTime);
        
        void Update( IContext context, ILifeTime lifeTime);
    }
}