namespace UniGreenModules.UniNodeSystem.Runtime
{
    using System;
    using Connections;
    using Interfaces;
    using UniCore.Runtime.Common;
    using UniCore.Runtime.Interfaces;
    using UniCore.Runtime.ObjectPool.Interfaces;
    using UniRx;

    [Serializable]
    public class UniPortValue : IPortValue , IPoolable
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

        [NonSerialized] private TypeDataBrodcaster context;

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
            context = new TypeDataBrodcaster();
            
            valueObservable = new TypeValueObservable<UniPortValue>();
            valueObservable.Initialize(this);
            //register observable as broadcast target
            context.Connect(valueObservable);
            
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
                context.Remove<TData>();
            }
            return result;
        }

        public void Add<TData>(TData value)
        {
            typeData.Add(value);
            context.Add(value);
            messageBroker.Publish(value);
        }

        public void CleanUp()
        {
            typeData.CleanUp();
        }

        public void RemoveAllConnections()
        {
            context.CleanUp();
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
            context.Connect(contextData);
            return this;
        }

        public void Disconnect(IContextWriter contextData)
        {
            context.Disconnect(contextData);
        }

        
        public void Release()
        {
            CleanUp();
            RemoveAllConnections();
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