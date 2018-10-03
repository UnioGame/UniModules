using System.Collections;
using Assets.Tools.UnityTools.ProfilerTools;
using UnityEngine;

namespace Assets.Tools.UnityTools.StateMachine
{
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
