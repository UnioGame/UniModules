using System;
using Assets.Tools.UnityTools.Interfaces;
using Assets.Tools.UnityTools.ObjectPool.Scripts;
using UniRx;
using UnityTools.Common;
using UnityTools.Interfaces;
using UnityTools.RecycleRx;

namespace Assets.Tools.UnityTools.Common
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
            Release();
            
            this.Despawn();
        }

        public IDisposable Subscribe(IObserver<TData> action)
        {
            return _reactiveValue.Subscribe(action);
        }
        
        #region IDataTransition
        
        public void CopyTo(IMessagePublisher target)
        {
            target.Publish(Value);
        }
        
        #endregion
        
        private void Release()
        {
            _reactiveValue.Release();
        }

    }
}