using System.Collections;
using UniModule.UnityTools.Interfaces;
using UnityEngine;

namespace UniStateMachine.CommonNodes
{
    using UniGreenModules.UniCore.Runtime.Interfaces;
    using UniGreenModules.UniNodeSystem.Runtime;

    public class FpsLimitNode : UniNode
    {

        public int FpsLimit = 60;


        protected override IEnumerator OnExecuteState(IContext context)
        {
            
            SetTargetFrameRate(FpsLimit);
            yield return base.OnExecuteState(context);
            
        }
        
        private void SetTargetFrameRate(int fps)
        {
            Application.targetFrameRate = fps;
        }
        
    }
}
