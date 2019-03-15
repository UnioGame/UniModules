

using UniModule.UnityTools.ObjectPool.Scripts;

namespace UniStateMachine.Nodes
{
    using System;
    using UniModule.UnityTools.Common;
    using UniModule.UnityTools.UniVisualNodeSystem.Connections;
    using UniRx;

    public class TypeValueObservable<TTarget> : 
        IContextWriter, 
        ITypeValueObservable
    
        where TTarget : class,
        ITypeDataContainer, 
        IConnector<IContextWriter>
    {
        private TTarget _target;
        
        private readonly ReactiveProperty<TypeValueUnit> _dataUpdate;
        private readonly ReactiveProperty<TypeValueUnit> _dataRemove;
        private readonly Subject<ITypeDataContainer> _emptyDataObservable;

        public IObservable<TypeValueUnit> UpdateValueObservable => _dataUpdate;

        public IObservable<TypeValueUnit> DataRemoveObservable => _dataRemove;

        public IObservable<ITypeDataContainer> EmptyDataObservable => _emptyDataObservable;

        
        public TypeValueObservable(TTarget target)
        {
            _target = target;
            _target.Connect(this);
            
            _dataUpdate = new ReactiveProperty<TypeValueUnit>();
            _dataRemove = new ReactiveProperty<TypeValueUnit>();
            _emptyDataObservable = new Subject<ITypeDataContainer>();
            
        }

        public bool Remove<TData>()
        {
            return Remove(typeof(TData));
        }

        public bool Remove(Type type)
        {
            _dataRemove.Value = new TypeValueUnit()
            {
                Container = _target,
                ValueType = type,
            };

            if (_target.HasValue() == false)
            {
                _emptyDataObservable.OnNext(_target);
            }

            return true;
        }

        public void Add<TData>(TData data)
        {
            _dataUpdate.Value = (new TypeValueUnit()
            {
                Container = _target,
                ValueType = typeof(TData),
            });
        }

    }
}