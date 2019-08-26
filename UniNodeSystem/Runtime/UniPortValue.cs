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
    public class UniPortValue : IPortValue, IPoolable
    {
        #region serialized data

        /// <summary>
        /// port value Name
        /// </summary>
        public string name = string.Empty;

        #endregion

        #region private property

        [NonSerialized] private ITypeData typeData;

        [NonSerialized] private ITypeDataBrodcaster broadcaster;

        [NonSerialized] private bool initialized = false;
        
        [NonSerialized] private ReactiveCommand portValueChanged = new ReactiveCommand();

        #endregion

        #region public properties

        public string ItemName => name;

        public bool HasValue => typeData.HasValue;

        public IObservable<Unit> PortValueChanged => portValueChanged;
        
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
            var result = typeData.Remove<TData>();
            if (result) {
                broadcaster.Remove<TData>();
            }

            return result;
        }

        public void Add<TData>(TData value)
        {
            
            typeData.Add(value);
            broadcaster.Add(value);

            portValueChanged.Execute(Unit.Default);
            
        }

        public void CleanUp()
        {
            typeData.CleanUp();
            broadcaster.CleanUp();
        }

        public void RemoveAllConnections()
        {
            broadcaster.CleanUp();
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
        
        public IObservable<TData> GetObservable<TData>()
        {
            return typeData.GetObservable<TData>();
        }
        
        #endregion

        private void Initialize()
        {
            if (initialized)
                return;
            typeData    = new TypeData();
            broadcaster = new TypeDataBrodcaster();
            //mark as initialized
            initialized = true;
        }

     }
}