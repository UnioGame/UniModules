namespace Taktika.Addressables.Reactive
{
    using System;
    using System.Collections;
    using UniGreenModules.UniCore.Runtime.Attributes;
    using UniGreenModules.UniCore.Runtime.DataFlow;
    using UniGreenModules.UniCore.Runtime.ObjectPool.Runtime.Extensions;
    using UniGreenModules.UniCore.Runtime.ProfilerTools;
    using UniGreenModules.UniGame.AddressableTools.Runtime.Extensions;
    using UniGreenModules.UniGame.Core.Runtime.Rx;
    using UniGreenModules.UniRoutine.Runtime;
    using UniGreenModules.UniRoutine.Runtime.Extension;
    using UniRx;
    using UnityEngine;
    using UnityEngine.AddressableAssets;
    using UnityEngine.ResourceManagement.AsyncOperations;
    using Object = UnityEngine.Object;

    [Serializable]
    public class AddressableObservable<TAddressable,TData,TApi> : 
        IAddressableObservable<TApi> 
        where TAddressable : AssetReference 
        where TData : Object
        where TApi : class
    {
        private static Type componentType = typeof(Component);
        
        #region inspector

        [SerializeField] protected RecycleReactiveProperty<TApi> value = new RecycleReactiveProperty<TApi>();

        [SerializeField] protected TAddressable reference;

        [SerializeField] protected FloatRecycleReactiveProperty progress = new FloatRecycleReactiveProperty();
     
        #if ODIN_INSPECTOR
        [Sirenix.OdinInspector.InlineEditor(Sirenix.OdinInspector.InlineEditorModes.SmallPreview)]
        #endif
        [ReadOnlyValue]
        [SerializeField] protected TData asset;
        
        [SerializeField]
        protected LifeTimeDefinition lifeTimeDefinition = new LifeTimeDefinition();

        #endregion
       
        private bool releaseOnDispose = true;

        private RoutineHandler routineHandler;

        protected RecycleReactiveProperty<bool> isReady = new RecycleReactiveProperty<bool>();
        
        protected RecycleReactiveProperty<AsyncOperationStatus> status = new RecycleReactiveProperty<AsyncOperationStatus>();
        
        #region public properties

        public IReadOnlyReactiveProperty<AsyncOperationStatus> Status => status;
        
        public IReadOnlyReactiveProperty<float> Progress => progress;

        public IReadOnlyReactiveProperty<bool> IsReady => isReady;

        public IReadOnlyReactiveProperty<TApi> Value => value;
        
        #endregion
        
        #region public methods
        
        /// <summary>
        /// initialize property with target Addressable Asset 
        /// </summary>
        /// <param name="addressable"></param>
        public void Initialize(TAddressable addressable, bool releaseAssetOnDispose = true)
        {
            lifeTimeDefinition = lifeTimeDefinition ?? new LifeTimeDefinition();
            progress = progress ?? new FloatRecycleReactiveProperty();
            status = status ?? new RecycleReactiveProperty<AsyncOperationStatus>();
            value = value ?? new RecycleReactiveProperty<TApi>();
            
            lifeTimeDefinition.Release();
            
            reference = addressable;
            releaseOnDispose = releaseAssetOnDispose;
        }

        public IDisposable Subscribe(IObserver<TApi> observer)
        {
            if (reference?.Asset != false) {
                return value.Subscribe(observer);
            }

            routineHandler.Cancel();
            routineHandler = LoadReference().Execute();
            routineHandler.AddTo(lifeTimeDefinition.LifeTime);
            return value.Subscribe(observer);
        }

        public void Dispose()
        {
            lifeTimeDefinition.Terminate();
            
            status.Release();
            status.Value = AsyncOperationStatus.None;
            
            progress.Release();
            progress.Value = 0;
            
            isReady.Release();
            isReady.Value = false;
            
            if (releaseOnDispose) {
                reference?.ReleaseAsset();
            }
            
            reference = null;
            
            value.Release();
            value.Value = default;
            
            this.Despawn();
        }

        #endregion
        
        private IEnumerator LoadReference()
        {
            if (reference == null || reference.RuntimeKeyIsValid() == false) {
                GameLog.LogError($"AddressableObservable : LOAD Addressable Failled {reference}");
                status.Value = AsyncOperationStatus.Failed;
                yield break;
            }

            var targetType = typeof(TData);
            var isComponent = componentType.IsAssignableFrom(targetType);

            var routine = isComponent
                ? LoadHandle<GameObject>(x => value.Value = x.GetComponent<TApi>()) 
                : LoadHandle<TData>(x => value.Value = x as TApi);
            
            yield return routine;

        }

        private IEnumerator LoadHandle<TValue>(Action<TValue> result) 
            where TValue : Object
        {
                      
            var handler = reference.LoadAssetAsync<TValue>();
            
            while (handler.IsDone == false) {

                progress.Value = handler.PercentComplete;
                status.Value   = handler.Status;
                
                yield return null;

            }

            isReady.Value = true;
            
            handler.AddTo(lifeTimeDefinition.LifeTime);
            result(handler.Result);
        }
        
    }
}
