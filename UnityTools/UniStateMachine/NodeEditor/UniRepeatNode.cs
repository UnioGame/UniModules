using System.Collections;
using System.Collections.Generic;
using Assets.Tools.UnityTools.Interfaces;
using UniStateMachine;
using UniStateMachine.Nodes;
using UnityEngine;


namespace UniStateMachine {
    
    public class UniRepeatNode : UniNode
    {
        
        #region port
        
        [Input(ShowBackingValue.Always, ConnectionType.Multiple)]
        public UniPortValue Restart;
        
        #endregion

        protected override IEnumerator ExecuteState(IContext context) 
        {
            yield return base.ExecuteState(context);

            while (IsActive(context)) {
                
                var data = Restart.Get<IContext>(context);
                //here should be same unique id for all context line
                yield return null;

            }

        }
    }
    
}
