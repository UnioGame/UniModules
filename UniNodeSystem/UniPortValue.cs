using System;
using UniModule.UnityTools.Common;
using UniModule.UnityTools.UniVisualNodeSystem.Connections;

namespace UniStateMachine.Nodes
{
    [Serializable]
    public class UniPortValue : IPortValue
    {
        #region serialized data
        
        /// <summary>
        /// port value Name
        /// </summary>
        public string Name;
        
        #endregion
        
        #region private property

        [NonSerialized] private TypeData _typeData;
        
        [NonSerialized] private bool _initialized = false;

        [NonSerialized] private BroadcastTypeData _broadcastContext;

        [NonSerialized] private TypeValueObservable<UniPortValue> _valueObservable;

        #endregion       
        
        #region observable

        public IObservable<TypeDataChanged> UpdateValueObservable => _valueObservable.UpdateValueObservable;

        public IObservable<TypeDataChanged> DataRemoveObservable => _valueObservable.DataRemoveObservable;

        public IObservable<ITypeData> EmptyDataObservable => _valueObservable.EmptyDataObservable;
        
        #endregion

        public UniPortValue()
        {
            Initialize();
        }

        public void Initialize()
        {
            if (_initialized)
                return;

            _typeData = new TypeData();
            _broadcastContext = new BroadcastTypeData();
            
            _valueObservable = new TypeValueObservable<UniPortValue>();
            _valueObservable.Initialize(this);
            //register observable as broadcast target
            _broadcastContext.Connect(_valueObservable);
            
            //mark as initialized
            _initialized = true;
        }
        
        public void ConnectToPort(string portName)
        {
            Name = portName;
        }

        #region type data container
        
        public bool Remove<TData>()
        {
            var result = _typeData.Remove<TData>();
            if (result)
            {
                _broadcastContext.Remove<TData>();
            }
            return result;
        }

        public void Add<TData>(TData value)
        {
            _typeData.Add(value);
            _broadcastContext.Add(value);
        }

        public void RemoveAll()
        {
            _typeData.RemoveAll();
            _broadcastContext.RemoveAll();
        }
                       
        public bool HasValue()
        {
            return _typeData.HasValue();
        }
        
        public TData Get<TData>()
        {
            return _typeData.Get<TData>();
        }

        public bool Contains<TData>()
        {
            return _typeData.Contains<TData>();
        }
        
        #endregion       
      
        #region connector
        
        public IConnector<IContextWriter> Connect(IContextWriter contextData)
        {
            _broadcastContext.Connect(contextData);
            return this;
        }

        public void Disconnect(IContextWriter contextData)
        {
            _broadcastContext.Disconnect(contextData);
        }

        #endregion

    }
}