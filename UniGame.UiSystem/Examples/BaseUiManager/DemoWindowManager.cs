using UniGreenModules.UniGame.UiSystem.Runtime;
using UnityEngine;

namespace UniGreenModules.UniGame.UiSystem.Examples.BaseUiManager
{
    using System;
    using System.Collections.Generic;
    using AddressableTools.Runtime.Attributes;
    using AddressableTools.Runtime.Extensions;
    using Taktika.Addressables.Reactive;
    using UniCore.Runtime.DataFlow;
    using UniCore.Runtime.ProfilerTools;
    using UniCore.Runtime.Rx.Extensions;
    using UniRx;
    using UnityEngine.AddressableAssets;

    public enum DemoUiType
    {
        Element,
        Screen,
        Window,
    }
    
    public class DemoWindowManager : MonoBehaviour
    {
        public GameUiViewManager uiViewManager;

        [ShowAssetReference]
        public AssetReference nextScene;

        public AssetReference demoAsset;

        public AssetReference demoAsset2;
        
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.Button]
#endif
        public void ShowDemoViewAsScreen(DemoUiType type,string tag = "")
        {
            switch (type) {
                case DemoUiType.Element:
                    uiViewManager.Open<DemoWindowView>(new ViewModelBase());
                    break;
                case DemoUiType.Screen:
                    uiViewManager.OpenScreen<DemoWindowView>(new ViewModelBase());
                    break;
                case DemoUiType.Window:
                    uiViewManager.OpenWindow<DemoWindowView>(new ViewModelBase());
                    break;
            }
        }

        public List<Object> firstAssets = new List<object>();
        private LifeTimeDefinition lifeTimeDefinition = new LifeTimeDefinition();
        
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.Button]
#endif
        public void LoadAddressables()
        {
            demoAsset.ToObservable<GameObject>().
                Do(x => firstAssets.Add(x)).
                Do(x => {
                    for (int i = 0; i < firstAssets.Count - 1; i++) {
                        var a1 = firstAssets[i];
                        var a2 = firstAssets[i + 1];

                        GameLog.Log($"AS1 == AS2 {a1 == a2}");
                    }
                }).
                Subscribe(x => GameLog.Log($"LOAD 1 ADRS {x.name}")).
                AddTo(lifeTimeDefinition.LifeTime);
            
        }
        
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.Button]
#endif
        public async void ReloadScene()
        {
            await nextScene.LoadSceneTaskAsync();
            nextScene.ReleaseAsset();
        }


        private void Start()
        {
            LoadAddressables();    
        }

        private void OnDisable()
        {
            lifeTimeDefinition.Terminate();
        }
    }
}
