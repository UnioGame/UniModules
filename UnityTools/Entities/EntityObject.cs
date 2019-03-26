using System;
using UniModule.UnityTools.Common;
using UniModule.UnityTools.DataFlow;
using UniModule.UnityTools.Interfaces;
using UniModule.UnityTools.UniPool.Scripts;
using UniModule.UnityTools.ProfilerTools;
using UniRx;

namespace UniModule.UnityTools.ActorEntityModel
{
    public class EntityObject : IContext
    {
        private IMessageBroker _broker;
        private TypeData _typeData;
        private LifeTimeDefinition _lifeTimeDefinition;

        #region public properties

        public ILifeTime LifeTime => _lifeTimeDefinition.LifeTime;
        
        #endregion
        
        public EntityObject()
        {
            //context data container
            _typeData = new TypeData();
            //create local message context
            _broker = MessageBroker.Default;
            //context lifetime
            _lifeTimeDefinition = new LifeTimeDefinition();
        }
       
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
            GameLog.LogMessage("ENTITY {0} PUBLISH: {1}",GetType().Name,message.GetType().Name);
            _broker.Publish(message);
        }

        public IObservable<T> Receive<T>()
        {
            GameLog.LogFormat("ENTITY REGISTER RECEIVE {0}: {1}",GetType().Name,typeof(T).Name);
            return _broker.Receive<T>();
        }

        #endregion

        #endregion

    }
}