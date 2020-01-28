namespace UniGreenModules.UniGameSystems.Runtime.Nodes
{
    using System.Collections.Generic;
    using Commands;
    using UniContextData.Runtime.Interfaces;
    using UniCore.Runtime.Interfaces;
    using UniCore.Runtime.ProfilerTools;
    using UniGame.AddressableTools.Runtime.Extensions;
    using UniGame.SerializableContext.Runtime.Addressables;
    using UniGame.SerializableContext.Runtime.AssetTypes;
    using UniGameFlow.UniNodesSystem.Assets.UniGame.UniNodes.Nodes.Runtime.Nodes;
    using UniRx.Async;
    using UnityEngine;

    [CreateNodeMenu("GameSystem/DataContext")]
    public class ContextDataNode : InOutPortNode
    {
        [Header("Context")]
        public ContextAssetReference contextAsset;

        [Header("Data Source")] 
        public AsyncContextDataSourceAssetReference contextDataSources;

        protected override void UpdateCommands(List<ILifeTimeCommand> nodeCommands)
        {
            base.UpdateCommands(nodeCommands);

            var outputContextTask = UniTask.FromResult<IContext>(PortPair.OutputPort);
            //create node commands
            var contextPortCommand = new RegisterDataSourceCommand(outputContextTask,contextAsset);
            var sourceOutputPortCommand = new RegisterDataSourceCommand(outputContextTask,contextDataSources);
            var registerContext = new RegisterDataSourceToContextAssetCommand(contextAsset,contextDataSources);
            
            nodeCommands.Add(registerContext);
            nodeCommands.Add(sourceOutputPortCommand);
            nodeCommands.Add(contextPortCommand);
        }
        
        
    }
}
