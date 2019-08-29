namespace UniGreenModules.UniContextData.Runtime.Entities
{
    using System;
    using UniCore.Runtime.Common;
    using UniCore.Runtime.DataFlow;
    using UniCore.Runtime.Interfaces;
    using UniCore.Runtime.Interfaces.Rx;
    using UniCore.Runtime.ObjectPool;
    using UniCore.Runtime.ProfilerTools;
    using UniCore.Runtime.Rx;
    using UniRx;

    public class EntityContext : IContext
    {
        private TypeData           _typeData;
        private LifeTimeDefinition _lifeTimeDefinition;
        private bool hasValue;

        public EntityContext()
        {
            //context data container
            _typeData = new TypeData();
            //context lifetime
            _lifeTimeDefinition = new LifeTimeDefinition();
        }

        
#region public properties

        public ILifeTime LifeTime => _lifeTimeDefinition.LifeTime;

        public bool HasValue => _typeData.HasValue;
        
#endregion
        
#region public methods

        public bool Contains<TData>()
        {
            return _typeData.Contains<TData>();
        }

        public virtual TData Get<TData>()
        {
            return _typeData.Get<TData>();
        }

        public bool Remove<TData>()
        {
            return _typeData.Remove<TData>();
        }

        public void CleanUp()
        {
            _typeData.CleanUp();
        }

        public void Release()
        {
            _typeData.Release();
            _lifeTimeDefinition.Release();
        }

        public virtual void Dispose()
        {
            Release();
            this.Despawn();
        }

#region rx 

        public void Publish<T>(T message)
        {
            _typeData.Publish(message);
        }

        public IObservable<T> Receive<T>()
        {
            return _typeData.Receive<T>();
        }

#endregion

#endregion

    }
}