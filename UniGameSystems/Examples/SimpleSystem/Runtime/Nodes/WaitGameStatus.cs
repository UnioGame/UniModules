namespace UniGreenModules.UniGameSystems.Examples.SimpleSystem.Nodes
{
    using Runtime.Context;
    using UniCore.Runtime.ProfilerTools;
    using UniCore.Runtime.Rx.Extensions;
    using UniGameFlow.UniNodesSystem.Assets.UniGame.UniNodes.Nodes.Runtime.Nodes;
    using UniRx;

    [CreateNodeMenu("Examples/DemoSystem/WaitGameStatus")]
    public class WaitGameStatus : InOutPortNode
    {
        protected override void OnExecute()
        {
            
            PortPair.InputPort.Receive<IDemoGameStatus>().
                Do(x => GameLog.Log("DATA IDemoGameStatus Received")).
                ContinueWith(x => x.IsGameReady).
                Where(x => x).
                Do(x => PortPair.OutputPort.Publish(x)).
                Do(x => GameLog.Log("GAME INITIALIZED")).
                Subscribe().
                AddTo(LifeTime);
            
        }
    }
}
