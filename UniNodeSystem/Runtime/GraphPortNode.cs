namespace UniGreenModules.UniNodeSystem.Runtime
{
    using System.Collections;
    using Interfaces;
    using Runtime;
    using UniCore.Runtime.Interfaces;
    using UnityEngine;

    public class GraphPortNode : UniNode, IGraphPortNode
    {
        [SerializeField]
        private PortIO direction;
        
        public PortIO Direction => direction;

        public IPortValue PortValue => direction == PortIO.Input ? Input : Output;

        public override string GetName()
        {
            return GetFormatedInputName(base.GetName());
        }

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
