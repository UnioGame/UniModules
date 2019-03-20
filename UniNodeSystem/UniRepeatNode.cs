using System.Collections;
using System.Collections.Generic;
using UniModule.UnityTools.Interfaces;
using UniNodeSystem;
using UniStateMachine;
using UniStateMachine.Nodes;
using UnityEngine;


namespace UniStateMachine {
    
    public class UniRepeatNode : UniNode
    {
        
        #region port
        
        [NodeInput(ShowBackingValue.Always, ConnectionType.Multiple)]
        public UniPortValue Restart;
        
        #endregion

        protected override IEnumerator ExecuteState(IContext context) 
        {
            yield return base.ExecuteState(context);

            while (true) {
                
                var data = Restart.Get<IContext>();
                //here should be same unique id for all context line
                yield return null;

            }

        }
    }
    
}
