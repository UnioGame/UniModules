using UniGreenModules.UniCore.Runtime.DataFlow.Interfaces;
using UniGreenModules.UniCore.Runtime.Interfaces;
using UniGreenModules.UniGameSystem.Runtime;

namespace Examples.SimpleSystem.Runtime
{
    public class SimpleSystem4 : GameService
    {
        protected override IContext OnBind(IContext context, ILifeTime lifeTime = null)
        {
            context.Publish(this);
            return context;
        }

        private void Initialize()
        {
            
        }
        
    }
}
