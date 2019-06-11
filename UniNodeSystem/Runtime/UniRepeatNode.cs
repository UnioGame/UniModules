using System.Collections;
using System.Collections.Generic;
using UniModule.UnityTools.Interfaces;
using UniNodeSystem;
using UniStateMachine;
using UniStateMachine.Nodes;
using UnityEngine;


namespace UniStateMachine {
    using UniGreenModules.UniCore.Runtime.Interfaces;

    public class UniRepeatNode : UniNode
    {
        
        #region port
        
        [NodeInput(ShowBackingValue.Always, ConnectionType.Multiple)]
        public UniPortValue Restart;
        
        #endregion

        protected override IEnumerator OnExecuteState(IContext context) 
        {
            yield return base.OnExecuteState(context);

            while (true) {
                
                var data = Restart.Get<IContext>();
                //here should be same unique id for all context line
                yield return null;

            }

        }
    }
    
}
