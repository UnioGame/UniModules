using System.Collections;
using System.Collections.Generic;
using UniModule.UnityTools.Interfaces;
using UnityEngine;

namespace UniStateMachine
{
    public class UniRootNode : UniGraphNode
    {
        protected override IEnumerator ExecuteState(IContext context)
        {
            
            while (true)
            {
                yield return base.ExecuteState(context);
                yield return null;
            }
           
        }
    }
}
