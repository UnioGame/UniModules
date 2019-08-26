namespace UniGreenModules.UniNodeSystem.Nodes
{
    using System.Collections.Generic;
    using Commands;
    using Runtime;
    using Runtime.Extensions;
    using Runtime.Interfaces;
    using Runtime.Runtime;
    using UniCore.Runtime.Interfaces;
    using UnityEngine;

    public class UniPortNode : UniNode, IUniPortNode
    {
                
#region inspector
        
        public PortIO direction = PortIO.Input;
        
#endregion

        private ConnectedFormatedPairCommand portPairCommand = new ConnectedFormatedPairCommand();

        public PortIO Direction => direction;

        public IPortValue PortValue { get; protected set; }

        public bool Visible => false;

        protected override void UpdateNodeCommands(List<ILifeTimeCommand> nodeCommands)
        {
            base.UpdateNodeCommands(nodeCommands);
            
            portPairCommand.Initialize(this, ItemName, true);
            PortValue = Direction == PortIO.Input ? 
                portPairCommand.InputPort : 
                portPairCommand.OutputPort;
            
            nodeCommands.Add(portPairCommand);
        }
    }
        
}
