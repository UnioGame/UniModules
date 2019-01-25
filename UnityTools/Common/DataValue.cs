using System;
using UniModule.UnityTools.Interfaces;
using UniModule.UnityTools.ObjectPool.Scripts;
using UniModule.UnityTools.ProfilerTools;
using UniModule.UnityTools.RecycleRx;
using UniRx;

namespace UniModule.UnityTools.Common
{
    [Serializable]
    public class ContextValue<TData> : 
        IDataValue<TData>, 
        IWritableValue, 
        IObservable<TData>
    {
        protected RecycleObservable<TData> _reactiveValue = new RecycleObservable<TData>();

        public TData Value
        {
            get => _reactiveValue.Value;
            protected set
            {
                _reactiveValue.SetValue(value);
            }
        }

        public void SetValue(TData value)
        {
            Value = value;
        }

        public void Dispose()
        {
            OnRelease();
            
            this.Despawn();
        }

        public IDisposable Subscribe(IObserver<TData> action)
        {
            return _reactiveValue.Subscribe(action);
        }
        
        #region IDataTransition
        
        public void CopyTo(IMessagePublisher target)
        {
            GameProfiler.BeginSample("DataValue_CopyTo");
            
            target.Publish(Value);
            
            GameProfiler.EndSample();
        }
        
        #endregion
        
        protected virtual void OnRelease()
        {
            _reactiveValue.Release();
        }

    }
}