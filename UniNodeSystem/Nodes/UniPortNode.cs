namespace UniGreenModules.UniNodeSystem.Nodes
{
    using System.Collections.Generic;
    using Commands;
    using Runtime;
    using Runtime.Core;
    using Runtime.Extensions;
    using Runtime.Interfaces;
    using UniCore.Runtime.Interfaces;
    using UnityEngine;

    public class UniPortNode : UniNode, IUniPortNode
    {
                
#region inspector
        
        public PortIO direction = PortIO.Input;

        public bool bindInOut = true;
        
#endregion

        private ConnectedFormatedPairCommand portPairCommand = new ConnectedFormatedPairCommand();

        public PortIO Direction => direction;

        public IPortValue PortValue { get; protected set; }

        public bool Visible => false;

        protected override void UpdateCommands(List<ILifeTimeCommand> nodeCommands)
        {
            base.UpdateCommands(nodeCommands);
            
            portPairCommand.Initialize(this, ItemName, bindInOut);
            PortValue = Direction == PortIO.Input ? 
                portPairCommand.InputPort : 
                portPairCommand.OutputPort;
            
            nodeCommands.Add(portPairCommand);
        }
    }
        
}
