namespace UniGreenModules.UniGameSystems.Examples.SimpleSystem.Runtime.Nodes
{
    using System.Linq;
    using UniCore.Runtime.Rx.Extensions;
    using UniGameFlow.UniNodesSystem.Assets.UniGame.UniNodes.Nodes.Runtime.Nodes;
    using UniNodeSystem.Runtime.Core;
    using UniRx;

    [UniBaseNode.CreateNodeMenuAttribute("Examples/DemoSystem/WaitGameStatus")]
    public class WaitGameStatus : InOutPortsNode
    {
        protected override void OnExecute()
        {

            var portPair = inOutPorts.FirstOrDefault();
            if (portPair == null) return;
            
            portPair.InputPort.Receive<DemoSystemGameStatus>().
                Where(x => x.IsInitialized).
                Do(x => portPair.OutputPort.Publish(x)).
                Subscribe().
                AddTo(LifeTime);
            
        }
    }
}
