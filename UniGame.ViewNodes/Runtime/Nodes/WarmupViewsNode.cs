using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UniGame.UniNodes.Nodes.Runtime.Common;
using UniModules.UniCore.Runtime.DataFlow;
using UniModules.UniCore.Runtime.Rx.Extensions;
using UniModules.UniGame.ViewSystem.Runtime.Extensions;
using UniModules.UniGame.ViewSystem.Runtime.Settings;
using UniModules.UniGameFlow.NodeSystem.Runtime.Core.Attributes;
using UniRx;
using Unity.Collections;

namespace UniModules.UniGame.GameFlow.GameFlow.Runtime.Views.Nodes
{
    [Serializable]
    [CreateNodeMenu("ViewSystem/WarmupViews")]
    public class WarmupViewsNode : InOutPortBindNode
    {
        #region inspector

        public List<AssetReferenceViewSettings> viewSettings = new List<AssetReferenceViewSettings>();

        [ReadOnly]
        public bool warmupComplete = false;
        
        #endregion

        private LifeTimeDefinition _warmupLifeTime = new LifeTimeDefinition();
        
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.Button]
#endif
        public void Warmup()
        {
            Preload();
        }
        
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.Button]
#endif
        public void Release()
        {
            _warmupLifeTime.Terminate();
        }
        
        protected override void OnExecute()
        {
            var inputPort = PortPair.InputPort;
            inputPort.PortValueChanged
                .Where(x => !warmupComplete)
                .Subscribe(async x => await Preload())
                .AddTo(LifeTime);
        }

        private async UniTask Preload(AssetReferenceViewSettings settings)
        {
            await settings.Warmup(_warmupLifeTime);
        }

        private async UniTask Preload()
        {
            if (warmupComplete)
                return;
            
            _warmupLifeTime.AddTo(LifeTime);
            _warmupLifeTime.AddCleanUpAction(() => this.warmupComplete = false);
            
            foreach (var viewSetting in viewSettings)
            {
                Preload(viewSetting);
            }

            warmupComplete = true;
        }
        
    }
}
