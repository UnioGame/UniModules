namespace UniGreenModules.UniNodeSystem.Nodes
{
    using System;
    using Runtime;
    using Runtime.Extensions;
    using Runtime.Interfaces;
    using Runtime.Runtime;
    using UnityEngine;

    public class UniPortNode : UniNode, IUniPortNode
    {

        [SerializeField]
        private PortIO direction = PortIO.Input;
        
        public PortIO Direction => direction;

        public IPortValue PortValue { get; protected set; }

        public bool Visible => false;

        protected override void OnNodeInitialize()
        {
            this.UpdatePortValue(ItemName, direction);
            base.OnNodeInitialize();
        }

    }
        
}
