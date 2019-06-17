namespace UniGreenModules.UniNodes.CommonNodes
{
    using System.Collections;
    using UniCore.Runtime.Interfaces;
    using UniNodeSystem.Runtime;
    using UnityEngine;

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
