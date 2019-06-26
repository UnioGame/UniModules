namespace UniGreenModules.UniNodeSystem.Nodes {
    using System.Collections;
    using Runtime;
    using Runtime.Runtime;
    using UniCore.Runtime.Interfaces;

    public class UniRepeatNode : UniNode
    {
        
        #region port
        
        [NodeInput(ShowBackingValue.Always, ConnectionType.Multiple)]
        public UniPortValue Restart;
        
        #endregion

        protected override IEnumerator OnExecuteState(IContext context) 
        {
            yield return base.OnExecute(context);

            while (true) {
                
                var data = Restart.Get<IContext>();
                //here should be same unique id for all context line
                yield return null;

            }

        }
    }
    
}
