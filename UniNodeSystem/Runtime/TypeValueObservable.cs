namespace UniGreenModules.UniNodeSystem.Runtime
{
    using System;
    using Interfaces;
    using UniCore.Runtime.Interfaces;
    using UniCore.Runtime.ObjectPool;
    using UniCore.Runtime.ObjectPool.Interfaces;
    using UniRx;

    public class TypeValueObservable<TTarget> : 
        IContextWriter, 
        ITypeValueObservable,
        IPoolable,
        IDisposable
    
        where TTarget : class,
        ITypeData, 
        IConnector<IContextWriter>
    {
        private TTarget _target;
        
        private ReactiveProperty<TypeDataChanged> _dataUpdate;
        private ReactiveProperty<TypeDataChanged> _dataRemove;
        private Subject<ITypeData> _emptyDataObservable;

        public void Initialize(TTarget target)
        {
            
            Release();
            _target = target;
 
        }

        #region public properties
  
        public IObservable<TypeDataChanged> UpdateValueObservable => _dataUpdate;

        public IObservable<TypeDataChanged> DataRemoveObservable => _dataRemove;

        public IObservable<ITypeData> EmptyDataObservable => _emptyDataObservable;

        #endregion
        
        #region icontext wirter
        
        public bool Remove<TData>()
        {
            _dataRemove.Value = new TypeDataChanged()
            {
                Container = _target,
                ValueType = typeof(TData),
            };

            if (!_target.HasValue)
            {
                _emptyDataObservable.OnNext(_target);
            }

            return true;
        }

        public void Add<TData>(TData data)
        {
            _dataUpdate.Value = (new TypeDataChanged()
            {
                Container = _target,
                ValueType = typeof(TData),
            });
        }

        public void CleanUp()
        {
            _emptyDataObservable.OnNext(_target);
        }
        
        #endregion

        public void Release()
        {
            _dataUpdate?.Dispose(); 
            _dataRemove?.Dispose();
            _emptyDataObservable?.Dispose();

            _target = null;
            
            _dataUpdate = new ReactiveProperty<TypeDataChanged>();
            _dataRemove = new ReactiveProperty<TypeDataChanged>();
            _emptyDataObservable = new Subject<ITypeData>();
        }

        public void Dispose()
        {
            Release();
            this.Despawn();
        }

    }
}