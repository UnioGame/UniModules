using System.Collections;
using UnityEngine;

namespace UniModule.UnityTools.UniStateMachine
{
    using UniGreenModules.UniCore.Runtime.ProfilerTools;

    public class DummyStateBehaviour : StateBehaviour
    {
        
        protected override IEnumerator ExecuteState()
        {
            GameLog.Log("IM DUMMY STATE",Color.cyan);
            while (IsActive.Value)
            {
                yield return null;
            }
        }


    }
}
