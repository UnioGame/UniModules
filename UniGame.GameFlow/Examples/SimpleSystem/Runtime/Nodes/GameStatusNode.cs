namespace UniGreenModules.UniGameSystems.Examples.SimpleSystem.Nodes
{
    using Runtime.Context;
    using UniCore.Runtime.Attributes;
    using UniCore.Runtime.Interfaces;
    using UniCore.Runtime.ProfilerTools;
    using UniCore.Runtime.Rx.Extensions;
    using UniGameFlow.UniNodesSystem.Assets.UniGame.UniNodes.Nodes.Runtime.Nodes;
    using UniRx;
    using UnityEngine;

    [CreateNodeMenuAttribute("Examples/DemoSystem/GameStatusNode")]
    public class GameStatusNode : ContextNode
    {
        [ReadOnlyValue]
        [SerializeField]
        public bool isGameReady = false;

        protected override void OnDataUpdated(IContext context, IContext source, IContext target)
        {
            context.Receive<SimpleSystem1>().
                CombineLatest(
                    context.Receive<SimpleSystem2>(), 
                    context.Receive<SimpleSystem3>(), 
                    context.Receive<SimpleSystem4>(),
                    (x, y, z, k) => context).
                Do(x => GameLog.Log("Game Services Ready")).
                Select(x => x.Receive<IDemoGameStatus>()).
                Switch().
                Do(x => GameLog.Log($"DemoGameStatus has value {x.IsGameReady.HasValue} is Ready {x.IsGameReady.Value}")).
                Do(x => isGameReady = x.IsGameReady.Value).
                Do(x => GameLog.Log("Game Status: Ready")).
                Do(x => x.SetGameStatus(true)).
                Do(x => base.OnDataUpdated(context,source, target)).
                Subscribe().
                AddTo(LifeTime);
        }
    }
}
