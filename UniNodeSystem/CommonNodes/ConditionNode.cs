using System.Collections;
using System.Collections.Generic;
using UniModule.UnityTools.Interfaces;
using UniModule.UnityTools.UniStateMachine.Extensions;
using UniStateMachine.Nodes;
using UnityEngine;
using UniNodeSystem;

namespace UniStateMachine.CommonNodes
{
    public class ConditionNode : UniNode
    {
        private const string _truePort = "True";
        private const string _falsePort = "False";

        protected UniPortValue _trueOuputValue;
        protected UniPortValue _falseOutputValue;

        protected override IEnumerator ExecuteState(IContext context)
        {
            
            yield return base.ExecuteState(context);

            while (true)
            {
                var result = MakeDecision(context);
                
                UpdateResultPort(_trueOuputValue, result, context);
                UpdateResultPort(_falseOutputValue, !result, context);

                yield return null;
            }
            
        }

        private void UpdateResultPort(UniPortValue portValue,bool result,IContext context)
        {
            if (result)
            {
                portValue.UpdateValue(context,context);
            }
            else
            {
                portValue.RemoveContext(context);
            }

        }
        
        protected virtual bool MakeDecision(IContext context)
        {
            return false;
        }

        protected override void OnUpdatePortsCache()
        {           
            base.OnUpdatePortsCache();

            _trueOuputValue = this.UpdatePortValue(_truePort, PortIO.Output).value;
            _falseOutputValue = this.UpdatePortValue(_falsePort, PortIO.Output).value;
        }
    }
}
