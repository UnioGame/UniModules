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

        [SerializeField]
        private PortIO direction;
        
        public PortIO Direction => direction;

        public IPortValue PortValue => direction == PortIO.Input ? Input : Output;

        protected override IEnumerator OnExecuteState(IContext context)
        {
            yield break;
        }

        protected override void OnUpdatePortsCache()
        {
            base.OnUpdatePortsCache();
            //redirect all input into output
            PortValue.Connect(Output);
        }

    }
    
}
