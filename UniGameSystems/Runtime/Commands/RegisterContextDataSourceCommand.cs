using UnityEngine;

namespace UniGreenModules.UniGameSystems.Runtime.Commands
{
    using System;
    using UniCore.Runtime.DataFlow.Interfaces;
    using UniCore.Runtime.Interfaces;
    using UniGame.SerializableContext.Runtime.Addressables;

    [Serializable]
    public class RegisterContextDataSourceCommand : ILifeTimeCommand
    {
        private readonly IContext context;
        private readonly ContextDataSourceAssetReference resource;
        
        [SerializeField]
        private string outputPortName;

        public RegisterContextDataSourceCommand(IContext context,ContextDataSourceAssetReference resource)
        {
            this.context = context;
            this.resource = resource;
        }
                
        public string OutputName { get; }

        public async void Execute(ILifeTime lifeTime)
        {
            var handler = resource.LoadAssetAsync();
            var asset = await handler.Task;
            await asset.RegisterAsync(context);
        }
    }
}
