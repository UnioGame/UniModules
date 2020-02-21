namespace Taktika.Addressables.Reactive
{
    using System;
    using System.Collections;
    using UniGreenModules.UniCore.Runtime.Attributes;
    using UniGreenModules.UniCore.Runtime.DataFlow;
    using UniGreenModules.UniCore.Runtime.ObjectPool.Runtime.Extensions;
    using UniGreenModules.UniCore.Runtime.ProfilerTools;
    using UniGreenModules.UniGame.Core.Runtime.Rx;
    using UniGreenModules.UniRoutine.Runtime;
    using UniGreenModules.UniRoutine.Runtime.Extension;
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

        private RoutineHandler routineHandler;

        private bool releaseOnDispose = true;
        
        protected RecycleReactiveProperty<AsyncOperationStatus> status = new RecycleReactiveProperty<AsyncOperationStatus>();
        
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

            if (releaseOnDispose) {
                reference?.ReleaseAsset();
            }
            reference = null;
            
            value.Release();
            value.Value = default;
            
            this.Despawn();
        }

        private IEnumerator LoadReference()
        {
            if (reference == null || reference.RuntimeKeyIsValid() == false) {
                GameLog.LogError($"AddressableObservable : LOAD Addressable Failled {reference}");
                status.Value = AsyncOperationStatus.Failed;
                yield break;
            }

            var handler = reference.LoadAssetAsync<Object>();
            while (handler.IsDone == false) {

                progress.Value = handler.PercentComplete;
                status.Value = handler.Status;
                
                yield return null;

            }
            
            value.Value = handler.Result as TApi;
        }
        
    }
}
