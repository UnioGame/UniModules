using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UniModules.UniCore.Runtime.DataFlow;
using UniModules.UniGame.GameFlow.GameFlow.Runtime.Nodes.Common;
using UniModules.UniGame.ViewSystem.Runtime.Extensions;
using UniModules.UniGame.ViewSystem.Runtime.Settings;
using UniModules.UniGameFlow.NodeSystem.Runtime.Core.Attributes;

namespace UniModules.UniGame.GameFlow.GameFlow.Runtime.Views.Nodes
{
    [Serializable]
    [CreateNodeMenu("ViewSystem/WarmupViews S")]
    public class WarmupViewsSNode : ProxyPortSNode
    {
        #region inspector

        public int value;
        
        public bool one;
        
        public List<AssetReferenceViewSettings> viewSettings = new List<AssetReferenceViewSettings>();
        
        #endregion

        private LifeTimeDefinition _warmupLifeTime = new LifeTimeDefinition();

        protected override async void OnExecute()
        {
            _warmupLifeTime.Release();
            _warmupLifeTime.AddTo(LifeTime);

            foreach (var viewSetting in viewSettings)
            {
                Preload(viewSetting);
            }
            
            base.OnExecute();
        }

        private async UniTask Preload(AssetReferenceViewSettings settings)
        {
            await settings.Warmup(_warmupLifeTime);
        }
        
    }
}
