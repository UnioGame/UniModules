using UniGreenModules.UniCore.Runtime.DataFlow.Interfaces;
using UniGreenModules.UniCore.Runtime.Interfaces;
using UniGreenModules.UniGameSystem.Runtime;

namespace Examples.SimpleSystem.Runtime
{
    using System.Collections;
    using UniGreenModules.UniRoutine.Runtime;
    using UniGreenModules.UniRoutine.Runtime.Extension;

    public class SimpleSystem3 : GameService
    {
        
        protected override IContext OnBind(IContext context, ILifeTime lifeTime = null)
        {
            context.Publish(this);
            ReadyDelay(3).Execute();
            return context;
        }

        private IEnumerator ReadyDelay(float delay)
        {
            yield return this.WaitForSeconds(delay);
            isReady.Value = true;
        }
        
    }
}
