namespace UniGreenModules.UniNodeSystem.Runtime
{
    using System;
    using Connections;
    using Interfaces;
    using UniContextData.Runtime.Entities;
    using UniCore.Runtime.Common;
    using UniCore.Runtime.DataFlow;
    using UniCore.Runtime.Interfaces;
    using UniCore.Runtime.ObjectPool.Interfaces;
    using UniRx;

    [Serializable]
    public class UniPortValue : IPortValue, IPoolable
    {
        #region serialized data

        /// <summary>
        /// port value Name
        /// </summary>
        public string name = string.Empty;

        #endregion

        #region private property

        [NonSerialized] private IContext context;

        [NonSerialized] private ITypeDataBrodcaster broadcaster;

        [NonSerialized] private bool initialized = false;
        
        [NonSerialized] private ReactiveCommand portValueChanged = new ReactiveCommand();

#endregion

        #region public properties

        public string ItemName => name;

        public bool HasValue => context.HasValue;

        public IObservable<Unit> PortValueChanged => portValueChanged;

        public ILifeTime LifeTime => context.LifeTime;
        
        #endregion

        public UniPortValue()
        {
            Initialize();
        }

        public void ConnectToPort(string portName)
        {
            name = portName;
            Initialize();
        }

        public void Dispose()
        {
            Release();
        }

        #region type data container

        public bool Remove<TData>()
        {
            var result = context.Remove<TData>();
            if (result) {
                broadcaster.Remove<TData>();
                portValueChanged.Execute(Unit.Default);
            }

            return result;
        }

        public void Publish<TData>(TData value)
        {
            
            context.Publish(value);
            broadcaster.Publish(value);

            portValueChanged.Execute(Unit.Default);
            
        }

        public void CleanUp()
        {
            context.CleanUp();
            broadcaster.CleanUp();
        }

        public void RemoveAllConnections()
        {
            broadcaster.CleanUp();
        }

        public TData Get<TData>()
        {
            return context.Get<TData>();
        }

        public bool Contains<TData>()
        {
            return context.Contains<TData>();
        }

        #endregion

        #region connector

        public IConnector<IContextWriter> Connect(IContextWriter contextData)
        {
            broadcaster.Connect(contextData);
            return this;
        }

        public void Disconnect(IContextWriter contextData)
        {
            broadcaster.Disconnect(contextData);
        }

        public void Release()
        {
            CleanUp();
            RemoveAllConnections();
        }

        public IObservable<T> Receive<T>()
        {
            return context.Receive<T>();
        }
        
        #endregion

        private void Initialize()
        {
            if (initialized)
                return;
            context    = new EntityContext();
            broadcaster = new TypeDataBrodcaster();
            //mark as initialized
            initialized = true;
        }


    }
}