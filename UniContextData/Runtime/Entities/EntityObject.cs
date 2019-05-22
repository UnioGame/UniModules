namespace UniGreenModules.UniContextData.Runtime.Entities
{
    using System;
    using UniCore.Runtime.Common;
    using UniCore.Runtime.DataFlow;
    using UniCore.Runtime.Interfaces;
    using UniCore.Runtime.ObjectPool;
    using UniCore.Runtime.ProfilerTools;
    using UniCore.Runtime.Rx;
    using UniRx;

    public class EntityObject : IContext
    {
        private IRecycleMessageBrocker _broker;
        private TypeData           _typeData;
        private LifeTimeDefinition _lifeTimeDefinition;

        public EntityObject()
        {
            //context data container
            _typeData = new TypeData();
            //create local message context
            _broker = new RecycleMessageBrocker();
            //context lifetime
            _lifeTimeDefinition = new LifeTimeDefinition();
        }

        
#region public properties

        public ILifeTime LifeTime => _lifeTimeDefinition.LifeTime;

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

        public void RemoveAll()
        {
            _typeData.RemoveAll();
        }

        public void Add<TData>(TData data)
        {
            _typeData.Add(data);
            Publish(data);
        }

        public void Release()
        {
            _broker.Release();
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
            GameLog.LogMessage("ENTITY {0} PUBLISH: {1}", GetType().Name, message.GetType().Name);
            _broker.Publish(message);
        }

        public IObservable<T> Receive<T>()
        {
            GameLog.LogFormat("ENTITY REGISTER RECEIVE {0}: {1}", GetType().Name, typeof(T).Name);
            return _broker.Receive<T>();
        }

#endregion

#endregion
    }
}