using UniStateMachine;

namespace Tests.Examples
{
    using System.Collections;
    using UniGreenModules.UniCore.Runtime.Interfaces;
    using UniGreenModules.UniNodeSystem.Runtime;
    using UnityEngine;

    public class ContextMessageLogNode : UniNode
    {
        protected override IEnumerator OnExecuteState(IContext context)
        {
            var message = context.Get<string>();
    
            if(string.IsNullOrEmpty(message) == false)
                Debug.Log(message);
            
            yield return base.Execute(context);
            
        }
    }
}
