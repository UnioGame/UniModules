using System.Collections;
using System.Collections.Generic;
using UniGreenModules.UniNodeSystem.Runtime;
using UnityEngine;


namespace Tests.ReactivePorts
{
    using UniGreenModules.UniCore.Runtime.Interfaces;

    public class TestMessagePublisherNode : UniNode
    {
        public string message = "EMPTY MESSAGE";
        
        protected override IEnumerator OnExecuteState(IContext context)
        {
            Output.Add(message);
            yield break;   
        }
    }
}