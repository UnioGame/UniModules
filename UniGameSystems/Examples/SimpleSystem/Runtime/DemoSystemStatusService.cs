using UnityEngine;

namespace UniGreenModules.UniGameSystems.Examples.SimpleSystem.Runtime
{
    using Context;
    using global::Examples.SimpleSystem.Runtime;
    using UniCore.Runtime.DataFlow.Interfaces;
    using UniCore.Runtime.Interfaces;
    using UniCore.Runtime.ProfilerTools;
    using UniCore.Runtime.Rx.Extensions;
    using UniGameSystem.Runtime;
    using UniRx;

    public class DemoSystemStatusService : GameService
    {
        protected override IContext OnBind(IContext context, ILifeTime lifeTime)    
        {

            context.Receive<IDemoGameStatus>().
                Do(x => GameLog.Log($"DemoGameStatus has value {x.IsGameReady.HasValue} is Ready {x.IsGameReady.Value}")).
                Where(x => x!=null && x.IsGameReady.Value == false).
                Do(x => GameLog.Log("Mark Game Status as Ready")).
                Do(x => x.SetGameStatus(true)).
                Subscribe().
                AddTo(LifeTime);
                
            return context;
            
            context.Receive<SimpleSystem1>().
                CombineLatest(
                    context.Receive<SimpleSystem2>(), 
                    context.Receive<SimpleSystem3>(), 
                    context.Receive<SimpleSystem4>(),
                (x, y, z, k) => context).
                Do(x => GameLog.Log("Systems Messages received")).
                ContinueWith(x => x.Receive<IDemoGameStatus>()).
                Where(x => x.IsGameReady.Value == false).
                Do(x => GameLog.Log("Mark Game Status as Ready")).
                Do(x => x.SetGameStatus(true)).
                Subscribe().
                AddTo(lifeTime);
            
            return context;

        }

    }
}
