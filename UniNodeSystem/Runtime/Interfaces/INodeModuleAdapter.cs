namespace UniGreenModules.UniNodeSystem.Runtime.Interfaces
{
    using System;
    using System.Collections.Generic;
    using UniCore.Runtime.Interfaces;

    public interface INodeModuleAdapter
    {
        IReadOnlyCollection<PortDefinition> Ports { get; }

        IDisposable Bind(string portName,IContextData<IContext> portValue,IContext context);
        
        void Execute(string portName,IContextData<IContext> portValue, IContext context);
    }
}