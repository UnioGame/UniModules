using System.Collections;
using Assets.Tools.UnityTools.Interfaces;
using Assets.Tools.UnityTools.StateMachine.UniStateMachine;
using UnityEngine;

namespace GamePlay.States {
    
    [CreateAssetMenu(menuName = "States/States/DummyState", fileName = "DummyState")]
    public class DummyState : UniStateBehaviour {

        protected override IEnumerator ExecuteState(IContext context) {

            yield break;

        }

    }

}
