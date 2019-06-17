namespace UniGreenModules.UniNodeSystem.Examples.ActorReuse
{
    using System.Collections;
    using Runtime;
    using UniCore.Runtime.Interfaces;
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
