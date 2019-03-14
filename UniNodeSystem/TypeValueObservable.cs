

namespace UniStateMachine.Nodes
{
    using System;
    using UniModule.UnityTools.Common;
    using UniModule.UnityTools.UniVisualNodeSystem.Connections;
    using UniRx;

    public class TypeValueObservable<TTarget> : 
        ITypeDataWriter, 
        ITypeValueObservable
        where TTarget : ITypeDataContainer, IConnector<ITypeDataWriter>
    {
        private TTarget _target;
        
        private ReactiveProperty<TypeValueUnit> _dataUpdate = new ReactiveProperty<TypeValueUnit>();
        private ReactiveProperty<TypeValueUnit> _dataRemove = new ReactiveProperty<TypeValueUnit>();
        private Subject<ITypeDataContainer> _emptyDataObservable = new Subject<ITypeDataContainer>();

        
        public IObservable<TypeValueUnit> UpdateValueObservable => _dataUpdate;

        public IObservable<TypeValueUnit> DataRemoveObservable => _dataRemove;

        public IObservable<ITypeDataContainer> EmptyDataObservable => _emptyDataObservable;

        
        public void Initialize(TTarget target)
        {
            _target = target;
            _target.Connect(this);
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