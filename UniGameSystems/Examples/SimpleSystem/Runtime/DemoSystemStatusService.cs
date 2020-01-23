using UnityEngine;

namespace UniGreenModules.UniGameSystems.Examples.SimpleSystem.Runtime
{
    using global::Examples.SimpleSystem.Runtime;
    using UniCore.Runtime.DataFlow.Interfaces;
    using UniCore.Runtime.Interfaces;
    using UniCore.Runtime.Rx.Extensions;
    using UniGameSystem.Runtime;
    using UniRx;

    public class DemoSystemStatusService : GameService
    {
        protected override IContext OnBind(IContext context, ILifeTime lifeTime)
        {

            context.Receive<SimpleSystem1>().CombineLatest(
                context.Receive<SimpleSystem2>(),
                context.Receive<SimpleSystem3>(),
                context.Receive<SimpleSystem4>(),
                (x, y, z, k) => GetSystemStatus()).
                Do(context.Publish).
                Do(x => isReady.Value = true).
                Subscribe().
                AddTo(lifeTime);
            
            return context;

        }

        private DemoSystemGameStatus GetSystemStatus()
        {
            return new DemoSystemGameStatus() {
                IsInitialized = true,
            };
        }
    }
}
