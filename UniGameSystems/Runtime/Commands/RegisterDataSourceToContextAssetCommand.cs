using UnityEngine;

namespace UniGreenModules.UniGameSystems.Runtime.Commands
{
    using System;
    using UniCore.Runtime.DataFlow.Interfaces;
    using UniCore.Runtime.Interfaces;
    using UniCore.Runtime.ProfilerTools;
    using UniGame.AddressableTools.Runtime.Extensions;
    using UniGame.SerializableContext.Runtime.Addressables;
    using UniRx.Async;

    [Serializable]
    public class RegisterDataSourceToContextAssetCommand : ILifeTimeCommand
    {
        private readonly ContextAssetReference contextResource;
        private readonly AsyncContextDataSourceAssetReference resource;

        public RegisterDataSourceToContextAssetCommand(ContextAssetReference contextResource,AsyncContextDataSourceAssetReference resource)
        {
            this.contextResource = contextResource;
            this.resource = resource;
        }

        public async void Execute(ILifeTime lifeTime)
        {
            var context = await contextResource.LoadAssetTaskAsync();
            var asset = await resource.LoadAssetTaskAsync();
            
            if (!asset || !context) {
                GameLog.LogError($"NULL asset loaded from {resource} context {contextResource}");
                return;    
            }
            
            await asset.RegisterAsync(context.Value);
        }
    }
}
