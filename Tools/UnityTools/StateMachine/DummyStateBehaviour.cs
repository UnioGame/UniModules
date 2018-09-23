using System.Collections;
using Assets.Scripts.ProfilerTools;
using UnityEngine;

namespace UniStateMachine
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
