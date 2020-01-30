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

    [CreateNodeMenu("Examples/DemoSystem/GameStatusNode")]
    public class GameStatusNode : InOutPortNode
    {
        [ReadOnlyValue]
        [SerializeField]
        public bool isGameReady = false;
        
        protected override void OnExecute()
        {
            var context = PortPair.InputPort;
            
            context.Receive<IContext>().
                ContinueWith(x => x.Receive<IDemoGameStatus>()).
                Do(x => GameLog.Log($"DemoGameStatus has value {x.IsGameReady.HasValue} is Ready {x.IsGameReady.Value}")).
                Do(x => isGameReady = x.IsGameReady.Value).
                Do(x => GameLog.Log("Game Status: Ready")).
                Do(x => x.SetGameStatus(true)).
                Subscribe().
                AddTo(LifeTime);

            return;
            
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
                AddTo(LifeTime);
            
        }
    }
}
