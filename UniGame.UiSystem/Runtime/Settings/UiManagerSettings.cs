using UnityEngine;

namespace Taktika.MVVM.Runtime.Settings
{
    using System;
    using System.Collections.Generic;
    using Addressables.Reactive;
    using Sirenix.OdinInspector;
    using UniGreenModules.UniCore.Runtime.DataFlow;
    using UniGreenModules.UniCore.Runtime.ProfilerTools;
    using UniGreenModules.UniCore.Runtime.Rx.Extensions;
    using UniGreenModules.UniGame.AddressableTools.Runtime.Attributes;
    using UniRx;

    [CreateAssetMenu(menuName = "Taktika/Ui/UiSettings", fileName = "UiManagerSettings")]
    public class UiManagerSettings : UiViewsSource, IDisposable
    {
        private LifeTimeDefinition lifeTimeDefinition;
        
        [ShowAssetReference]
        [DrawWithUnity]
        public List<UiViewsSourceReference> sources = new List<UiViewsSourceReference>();

        public void Dispose()
        {
            lifeTimeDefinition?.Terminate();   
        }
        
        private void OnEnable()
        {
            lifeTimeDefinition = new LifeTimeDefinition();
            var lifeTime = lifeTimeDefinition.LifeTime;

            if (Application.isPlaying == false)
                return;
            
            //load ui views async
            foreach (var reference in sources) {
                reference.ToObservable().
                    Catch<UiViewsSource,Exception>(x => {
                        GameLog.LogError($"UiManagerSettings Load Ui Source failed {reference.AssetGUID}");
                        GameLog.LogError(x);
                        return Observable.Empty<UiViewsSource>();
                    }).
                    Where(x => x!=null).
                    Do(x => uiViews.AddRange(x.uiViews)).
                    Subscribe().AddTo(lifeTime);
            }
        }

        private void OnDisable()
        {
            Dispose();
        }

    }
}
