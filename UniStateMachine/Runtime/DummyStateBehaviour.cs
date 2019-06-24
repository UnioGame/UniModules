namespace UniGreenModules.UniStateMachine.Runtime
{
    using System.Collections;
    using UniCore.Runtime.ProfilerTools;
    using UnityEngine;

    public class DummyStateBehaviour : StateBehaviour
    {
        
        protected override IEnumerator ExecuteState()
        {
            GameLog.Log("IM DUMMY STATE",Color.cyan);
            while (IsActive)
            {
                yield return null;
            }
        }


    }
}
