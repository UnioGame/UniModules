using System;
using Assets.Tools.UnityTools.Interfaces;
using Assets.Tools.UnityTools.ObjectPool.Scripts;
using UniRx;
using UnityTools.Common;
using UnityTools.Interfaces;

namespace Assets.Tools.UnityTools.Common
{
    [Serializable]
    public class ContextValue<TData> : IDataValue<TData>, IWritableValue
    {
        protected ReactiveProperty<TData> _reactiveValue = new ReactiveProperty<TData>();
        protected bool _isReleased;

        public IReadOnlyReactiveProperty<TData> ReactiveValue => _reactiveValue;

        public TData Value
        {
            get => _reactiveValue.Value;
            protected set { _reactiveValue.Value = value; }
        }

        public void SetValue(TData value)
        {
            _isReleased = false;
            Value = value;
        }

        public void Dispose()
        {
            if(_isReleased)
                return;

            Release();
            
            this.Despawn();
        }
        
        #region IDataTransition
        
        public void CopyTo(IDataWriter target)
        {
            target.Add(Value);
        }
        
        #endregion
        
        private void Release()
        {
            Value = default(TData);
            _isReleased = true;
        }

    }
}