using System;
using UniModule.UnityTools.Common;
using UniModule.UnityTools.UniVisualNodeSystem.Connections;

namespace UniStateMachine.Nodes
{
    [Serializable]
    public class UniPortValue : 
        ITypeDataContainer, 
        ITypeValueObservable,
        IConnector<IContextWriter>
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

        public IObservable<TypeValueUnit> UpdateValueObservable => _valueObservable.UpdateValueObservable;

        public IObservable<TypeValueUnit> DataRemoveObservable => _valueObservable.DataRemoveObservable;

        public IObservable<ITypeDataContainer> EmptyDataObservable => _valueObservable.EmptyDataObservable;
        
        #endregion

        public UniPortValue()
        {
            Initialize();
        }

        public void Initialize()
        {
            if (_initialized)
                return;

            _valueObservable = new TypeValueObservable<UniPortValue>(this);
            _typeData = new TypeData();
            _broadcastContext = new BroadcastTypeData();
            
            _initialized = true;
        }
        
        public void ConnectToPort(string portName)
        {
            Name = portName;
        }

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

        #region broadcast

        public IConnector<IContextWriter> Connect(IContextWriter contextData)
        {
            _broadcastContext.Connect(contextData);
            return this;
        }

        public void Disconnect(IContextWriter contextData)
        {
            _broadcastContext.Remove(contextData);
        }

        #endregion

                
        public bool HasValue()
        {
            return _typeData.HasValue();
        }
        
        
        public TData Get<TData>()
        {
            return _typeData.Get<TData>();
        }

        
        #region connector

        public bool Contains<TData>()
        {
            return _typeData.Contains<TData>();
        }

        public bool Contains(Type type)
        {
            return _typeData.Contains(type);
        }
        
        #endregion


    }
}