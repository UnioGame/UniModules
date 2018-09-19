using System.Diagnostics;
using Assets.Scripts.ProfilerTools;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Assets.Scripts.Tools.StateMachine
{
    public class StateMachine<TAwaiter> :
        BaseStateMachine<IStateBehaviour<TAwaiter>>
    {

        #region constructor
        
        public StateMachine(IStateExecutor<IStateBehaviour<TAwaiter>> executor) : 
            base(executor) {}
        
        #endregion

        #region private methods

        protected override void OnExit() {
            State = null;
        }

        #endregion

    }
}
