namespace UniGreenModules.UniNodeSystem.Nodes
{
    using System.Collections;
    using Runtime;
    using Runtime.Interfaces;
    using Runtime.Runtime;
    using UniCore.Runtime.Interfaces;
    using UnityEngine;

    public class GraphPortNode : UniNode, IGraphPortNode
    {
        public bool visiblePort = true;
        
        public PortIO direction;
        
        public PortIO Direction => direction;

        public IPortValue PortValue => direction == PortIO.Input ? Input : Output;

        protected override IEnumerator OnExecuteState(IContext context)
        {
            yield break;
        }

        protected override void OnRegisterPorts()
        {
            base.OnRegisterPorts();
            //redirect all input into output
            Input.Connect(Output);
        }

        public bool Visible => visiblePort;

    }
    
}
