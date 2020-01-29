using UniGreenModules.UniGameFlow.UniNodesSystem.Assets.UniGame.UniNodes.Nodes.Runtime.Nodes;

namespace UniGreenModules.UniGameSystems.Runtime.Nodes
{
    using System.Collections.Generic;
    using Commands;
    using UniCore.Runtime.Interfaces;
    using UniCore.Runtime.ProfilerTools;
    using UniGame.AddressableTools.Runtime.Attributes;
    using UniGame.SerializableContext.Runtime.Addressables;
    using UniRx.Async;
    using UnityEngine;

    [CreateNodeMenu("GameSystem/Data Source")]
    public class ContextDataSourceNode : InOutPortNode
    {
        
        [Header("Node Output Data Source")]
        public AsyncContextDataSourceAssetReference contextDataSource;

        protected override void UpdateCommands(List<ILifeTimeCommand> nodeCommands)
        {
            base.UpdateCommands(nodeCommands);

            //create sync result for task
            var outputContextTask = UniTask.FromResult<IContext>(PortPair.OutputPort);
            //create node commands
            var sourceOutputPortCommand = new RegisterDataSourceCommand(outputContextTask,contextDataSource);
            
            nodeCommands.Add(sourceOutputPortCommand);
        }
    }
}
