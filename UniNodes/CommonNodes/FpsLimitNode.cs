using System.Collections;
using UniModule.UnityTools.Interfaces;
using UnityEngine;

namespace UniStateMachine.CommonNodes
{
    using UniGreenModules.UniCore.Runtime.Interfaces;

    public class FpsLimitNode : UniNode
    {

        public int FpsLimit = 60;


        protected override IEnumerator ExecuteState(IContext context)
        {
            
            SetTargetFrameRate(FpsLimit);
            yield return base.ExecuteState(context);
            
        }
        
        private void SetTargetFrameRate(int fps)
        {
            Application.targetFrameRate = fps;
        }
        
    }
}
