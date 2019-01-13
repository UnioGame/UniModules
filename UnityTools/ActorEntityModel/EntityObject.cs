using System;
using Assets.Modules.UnityToolsModule.Tools.UnityTools.DataFlow;
using Assets.Tools.UnityTools.Common;
using Assets.Tools.UnityTools.Interfaces;
using Assets.Tools.UnityTools.ObjectPool.Scripts;
using Assets.Tools.UnityTools.ProfilerTools;
using UniRx;

namespace UnityTools.ActorEntityModel
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

        public virtual TData Get<TData>()
        {
            return _typeData.Get<TData>();
        }

        public bool Remove<TData>()
        {
            return _typeData.Remove<TData>();
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

        #region rx 
                
        public void Publish<T>(T message)
        {
            GameLog.LogFormat("ENTITY {0} PUBLISH MESSAGE: {1}",GetType().Name,message.GetType().Name);
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