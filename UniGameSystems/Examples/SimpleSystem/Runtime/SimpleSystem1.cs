using UniGreenModules.UniCore.Runtime.DataFlow.Interfaces;
using UniGreenModules.UniCore.Runtime.Interfaces;
using UniGreenModules.UniGameSystem.Runtime;

namespace Examples.SimpleSystem.Runtime
{
    public class SimpleSystem1 : GameService
    {
        protected override IContext OnBind(IContext context, ILifeTime lifeTime = null)
        {
            context.Publish(this);
            isReady.Value = true;
            return context;
        }
        
    }
}
