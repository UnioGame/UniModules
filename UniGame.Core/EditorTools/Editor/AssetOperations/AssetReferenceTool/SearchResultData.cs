namespace UniModules.UniGame.EditorTools.Editor.AssetReferences
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using Core.EditorTools.Editor.EditorResources;
    using Core.Runtime.DataFlow.Interfaces;
    using Core.Runtime.Interfaces;
    using UniGreenModules.UniCore.EditorTools.Editor;
    using UniGreenModules.UniCore.Runtime.DataFlow;
    using UniGreenModules.UniCore.Runtime.Rx.Extensions;
    using UniRx;
    using Object = UnityEngine.Object;

    public class SearchResultData : IDisposable, ILifeTimeContext
    {
        private LifeTimeDefinition _lifeTime = new LifeTimeDefinition();
        private Subject<ProgressData> _progressData = new Subject<ProgressData>();
        
        #region constructor

        public SearchResultData()
        {
            _progressData.AddTo(_lifeTime);
        }
        
        #endregion
        
        public ConcurrentDictionary<Object, List<ResourceHandle>> referenceMap = new ConcurrentDictionary<Object, List<ResourceHandle>>();
        
        public AssetResourcesMap assets  = new AssetResourcesMap(2);

        public IObservable<ProgressData> Progression => _progressData;

        public ILifeTime LifeTime => _lifeTime;
        
        public void Dispose() => _lifeTime.Release();

        public SearchResultData AddKey(Object asset)
        {
            if (referenceMap.ContainsKey(asset)) {
                return this;
            }
            referenceMap[asset] = new List<ResourceHandle>();
            assets[asset]  = new EditorResource().Update(asset);
            return this;
        }

        public SearchResultData ReportProgress(ProgressData progress)
        {
            _progressData.OnNext(progress);
            return this;
        }

        public void Complete()
        {
            _lifeTime.Release();
            _progressData = new Subject<ProgressData>().AddTo(_lifeTime);
        }
    }
}