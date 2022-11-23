
using UniGame.UiSystem.Runtime.Settings;
using UniGame.UniNodes.Nodes.Runtime.Common;
using UniGame.Runtime.ObjectPool;
using UniGame.AddressableTools.Runtime;
using UniGame.Context.Runtime;
using UniModules.UniGame.CoreModules.UniGame.AddressableTools.Runtime.Pooling;
using UnityEngine;

namespace UniModules.UniGame.GameFlow.GameFlow.Runtime.Views.Nodes
{
    using System;
    using System.Collections.Generic;
    using Cysharp.Threading.Tasks;
    using global::UniGame.Core.Runtime;
    using UniModules.UniGame.ViewSystem.Runtime.Settings;
    using UniModules.UniGameFlow.NodeSystem.Runtime.Core.Attributes;
    
    [Serializable]
    [CreateNodeMenu("ViewSystem/WarmupViews",nodeName = "ViewsWarmupNode")]
    public class WarmupViewsNode : ContextNode
    {
        public List<AssetReferenceViewSettings> viewSettingsReferences = new List<AssetReferenceViewSettings>();

        public int preload = 1;
        
        protected override async UniTask OnContextActivate(IContext context)
        {
            //pooling now bind to this lifeTime
            //todo remove
            LifeTime.ApplyPoolAssetLifeTime();
            
            foreach (var viewSettingsReference in viewSettingsReferences)
            {
                await CreateViewPool(viewSettingsReference);
            }
        }

        private async UniTask CreateViewPool(AssetReferenceViewSettings viewSettings)
        {
            var settings = await viewSettings.LoadAssetTaskApiAsync<ScriptableObject,IViewsSettings>(LifeTime);
            foreach (var viewReference in settings.Views)
            {
                var view = viewReference.View;
                view.AttachPoolLifeTimeAsync(LifeTime, preload).Forget();
            }
        }
    }
}
