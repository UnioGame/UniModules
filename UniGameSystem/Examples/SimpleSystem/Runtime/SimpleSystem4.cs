using UniGreenModules.UniCore.Runtime.DataFlow.Interfaces;
using UniGreenModules.UniCore.Runtime.Interfaces;
using UniGreenModules.UniGameSystem.Runtime;

namespace Examples.SimpleSystem.Runtime
{
    using UniGreenModules.UniCore.Runtime.Rx.Extensions;
    using UniRx;

    public class SimpleSystem4 : GameService
    {
        protected override IContext OnBind(IContext context, ILifeTime lifeTime)
        {
            isReady.Value = false;
            context.Publish(this);
            
            context.Receive<SimpleSystem3>().
                Where(x => x != null).
                Do(x => isReady.Value = true).
                Subscribe().
                AddTo(lifeTime);
            
            return context;
        }

    }
}
