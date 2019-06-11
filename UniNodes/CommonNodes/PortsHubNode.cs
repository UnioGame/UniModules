using System.Collections;
using System.Collections.Generic;
using UniModule.UnityTools.Interfaces;
using UnityEngine;

namespace UniStateMachine.CommonNodes
{
    using UniGreenModules.UniNodeSystem.Runtime;
    using UniGreenModules.UniNodeSystem.Runtime.Extensions;

    public class PortsHubNode : UniNode
    {
        private const string portHubTemplate = "port{0}";

        public int PortsCount;

        protected override void OnUpdatePortsCache()
        {
            base.OnUpdatePortsCache();

            for (var i = 0; i < PortsCount; i++)
            {

                var portName = string.Format(portHubTemplate, i);
                this.CreatePortPair(portName,true);

            }
            
        }
    }
}
