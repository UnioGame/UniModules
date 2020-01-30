namespace UniGreenModules.UniGameSystems.Examples.SimpleSystem.Nodes
{
    using Runtime.Context;
    using UniCore.Runtime.Attributes;
    using UniCore.Runtime.ProfilerTools;
    using UniCore.Runtime.Rx.Extensions;
    using UniGameFlow.UniNodesSystem.Assets.UniGame.UniNodes.Nodes.Runtime.Nodes;
    using UniRx;
    using UnityEngine;

    [CreateNodeMenu("Examples/DemoSystem/WaitGameStatus")]
    public class WaitGameStatus : InOutPortNode
    {
        [ReadOnlyValue]
        [SerializeField]
        private bool isGameReady = false;
        
        protected override void OnExecute()
        {
            
            PortPair.InputPort.Receive<IDemoGameStatus>().
                DistinctUntilChanged().
                Do(x => GameLog.Log("DATA IDemoGameStatus Received")).
                Select(x => x.IsGameReady).
                Switch().
                Do(x => isGameReady = x).
                Where(x => x).
                Do(x => PortPair.OutputPort.Publish(x)).
                Do(x => GameLog.Log("GAME INITIALIZED")).
                Subscribe().
                AddTo(LifeTime);
            
        }
    }
}
