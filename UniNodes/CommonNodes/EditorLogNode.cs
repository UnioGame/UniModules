using UniGreenModules.UniNodeSystem.Runtime;

namespace UniGreenModules.UniNodes.CommonNodes
{
    using System.Collections;
    using UniCore.Runtime.Interfaces;
    using UnityEngine;

    public class EditorLogNode : UniNode
    {
        public string message;

        protected override IEnumerator OnExecuteState(IContext context)
        {

            if (Application.isEditor) {
                Debug.Log(message);
            }
            
            yield return base.OnExecuteState(context);
            
        }
    }
}
