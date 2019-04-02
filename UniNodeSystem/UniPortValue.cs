using System;
using System.Collections.Generic;
using Sirenix.Serialization;
using UniModule.UnityTools.Common;
using UniModule.UnityTools.UniVisualNodeSystem.Connections;
using UniRx;

namespace UniStateMachine.Nodes
{
    [Serializable]
    public class UniPortValue : IPortValue
    {
        #region serialized data
        
        /// <summary>
        /// port value Name
        /// </summary>
        public string name;

        #endregion
        
        #region private property

        [NonSerialized] private MessageBroker messageBroker;
        
        [NonSerialized] private TypeData typeData;
        
        [NonSerialized] private bool initialized = false;

        [NonSerialized] private BroadcastTypeData broadcastContext;

        [NonSerialized] private TypeValueObservable<UniPortValue> valueObservable;

        #endregion       
        
        #region observable

        public IObservable<TypeDataChanged> UpdateValueObservable => valueObservable.UpdateValueObservable;

        public IObservable<TypeDataChanged> DataRemoveObservable => valueObservable.DataRemoveObservable;

        public IObservable<ITypeData> EmptyDataObservable => valueObservable.EmptyDataObservable;
        
        #endregion

        public UniPortValue()
        {
            Initialize();
        }

        public void Initialize()
        {
            if (initialized)
                return;

            messageBroker = new MessageBroker();
            typeData = new TypeData();
            broadcastContext = new BroadcastTypeData();
            
            valueObservable = new TypeValueObservable<UniPortValue>();
            valueObservable.Initialize(this);
            //register observable as broadcast target
            broadcastContext.Connect(valueObservable);
            
            //mark as initialized
            initialized = true;
        }
        
        public void ConnectToPort(string portName)
        {
            name = portName;
        }

        #region type data container
        
        public bool Remove<TData>()
        {
            var result = typeData.Remove<TData>();
            if (result)
            {
                broadcastContext.Remove<TData>();
            }
            return result;
        }

        public void Add<TData>(TData value)
        {
            typeData.Add(value);
            broadcastContext.Add(value);
            messageBroker.Publish(value);
        }

        public void RemoveAll()
        {
            typeData.RemoveAll();
            broadcastContext.RemoveAll();
        }
                       
        public bool HasValue()
        {
            return typeData.HasValue();
        }
        
        public TData Get<TData>()
        {
            return typeData.Get<TData>();
        }

        public bool Contains<TData>()
        {
            return typeData.Contains<TData>();
        }
        
        #endregion       
      
        #region connector
        
        public IConnector<IContextWriter> Connect(IContextWriter contextData)
        {
            broadcastContext.Connect(contextData);
            return this;
        }

        public void Disconnect(IContextWriter contextData)
        {
            broadcastContext.Disconnect(contextData);
        }

        #endregion

        #region message receiver
        
        public IObservable<T> Receive<T>()
        {
            return messageBroker.Receive<T>();
        }
        
        #endregion
    }
}