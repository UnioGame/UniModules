namespace UniGame.Addressables.Reactive
{
    using System;
    using System.Collections;
    using UniCore.Runtime.ProfilerTools;
    using UniGreenModules.UniCore.Runtime.Attributes;
    using UniGreenModules.UniCore.Runtime.DataFlow;
    using UniGreenModules.UniCore.Runtime.DataFlow.Interfaces;
    using UniGreenModules.UniCore.Runtime.ObjectPool.Runtime;
    using UniGreenModules.UniCore.Runtime.ObjectPool.Runtime.Extensions;
    using UniGreenModules.UniCore.Runtime.ObjectPool.Runtime.Interfaces;
    using UniGreenModules.UniGame.AddressableTools.Runtime.Extensions;
    using UniGreenModules.UniGame.Core.Runtime.Common;
    using UniGreenModules.UniGame.Core.Runtime.Extension;
    using UniGreenModules.UniGame.Core.Runtime.Rx;
    using UniGreenModules.UniRoutine.Runtime;
    using UniRx;
    using UnityEngine;
    using UnityEngine.AddressableAssets;
    using UnityEngine.ResourceManagement.AsyncOperations;
    using Object = UnityEngine.Object;

    [Serializable]
    public class AddressableObservable<TAddressable,TData,TApi> : 
        IAddressableObservable<TApi> ,
        IPoolable
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
        public void Initialize(TAddressable addressable)
        {
            lifeTimeDefinition = lifeTimeDefinition ?? new LifeTimeDefinition();
            progress = progress ?? new FloatRecycleReactiveProperty();
            status = status ?? new RecycleReactiveProperty<AsyncOperationStatus>();
            value = value ?? new RecycleReactiveProperty<TApi>();
            
            lifeTimeDefinition.Release();
            
            reference = addressable;
            releaseOnDispose = true;
            lifeTimeDefinition.LifeTime.AddCleanUpAction(CleanUp);
            
        }

        public IDisposable Subscribe(IObserver<TApi> observer)
        {
            var disposableValue = value.Subscribe(observer);
            var disposableAction = ClassPool.Spawn<DisposableLifetime>();
            
            disposableAction.Initialize();
            disposableAction.AddDispose(disposableValue);
            
            routineHandler = LoadReference(disposableAction).
                Execute().
                AddTo(disposableAction);

            return disposableAction;
        }

        public void Dispose() {
            this.Despawn();
        }
        
        public void Release() => lifeTimeDefinition.Terminate();

        #endregion
        
        private IEnumerator LoadReference(ILifeTime lifeTime)
        {
            if (reference == null || reference.RuntimeKeyIsValid() == false) {
                GameLog.LogError($"AddressableObservable : LOAD Addressable Failled {reference}");
                status.Value = AsyncOperationStatus.Failed;
                yield break;
            }

            var targetType = typeof(TData);
            var apiType = typeof(TApi);

            var isComponent = targetType.IsComponent() || apiType.IsComponent();

            var routine = isComponent
                ? LoadHandle<GameObject>(lifeTime,x => value.Value = x.GetComponent<TApi>()) 
                : LoadHandle<TData>(lifeTime,x => value.Value = x as TApi);
            
            yield return routine;
        }

        private IEnumerator LoadHandle<TValue>(ILifeTime lifeTime,Action<TValue> result) 
            where TValue : Object
        {
            var handler = reference.
                LoadAssetAsync<TValue>().
                AddTo(lifeTime);
            
            while (handler.IsDone == false) {
                progress.Value = handler.PercentComplete;
                status.Value   = handler.Status;
                yield return null;
            }
            
            isReady.Value = true;
            result(handler.Result);
        }

        private void CleanUp()
        {
            status.Release();
            status.Value = AsyncOperationStatus.None;
            
            progress.Release();
            progress.Value = 0;
            
            isReady.Release();
            isReady.Value = false;
            
            reference = null;
            
            value.Release();
            value.Value = default;
        }
        
        #region deconstructor
        
        ~AddressableObservable() => Release();
        
        #endregion
    }
}
