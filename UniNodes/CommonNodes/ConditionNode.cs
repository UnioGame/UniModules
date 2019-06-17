namespace UniGreenModules.UniNodes.CommonNodes
{
    using System.Collections;
    using UniCore.Runtime.Interfaces;
    using UniNodeSystem.Runtime;
    using UniNodeSystem.Runtime.Extensions;
    using UniNodeSystem.Runtime.Runtime;

    public class ConditionNode : UniNode
    {
        private const string _truePort = "True";
        private const string _falsePort = "False";

        protected UniPortValue _trueOuputValue;
        protected UniPortValue _falseOutputValue;

        protected override IEnumerator OnExecuteState(IContext context)
        {
            
            yield return base.OnExecuteState(context);

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
                portValue.Add(context);
            }
            else
            {
                portValue.CleanUp();
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
