using System;
using UniModule.UnityTools.Common;
using UniModule.UnityTools.UniVisualNodeSystem.Connections;

namespace UniStateMachine.Nodes
{
    [Serializable]
    public class UniPortValue : 
        ITypeDataContainer, 
        IConnector<ITypeDataContainer>
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

        private BroadcastTypeData _broadcastContext;

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
            
            _initialized = true;
        }
        
        public void ConnectToPort(string portName)
        {
            Name = portName;
        }

        public bool Remove<TData>()
        {

        }

        public bool Remove(Type type)
        {
            var result = _typeData.Remove(type);
            if (result)
            {
                _broadcastContext.Remove(type);
            }

            return result;
        }

        public void Add<TData>(TData value)
        {
            _typeData.Add(value);
            _broadcastContext.Add(value);
        }

        #region broadcast

        public void Connect(ITypeDataContainer contextData)
        {
            _broadcastContext.Connect(contextData);
        }

        public void Remove(ITypeDataContainer contextData)
        {
            _broadcastContext.Remove(contextData);
        }

        #endregion

        #region reader

        public TData Get<TData>()
        {
            return _typeData.Get<TData>();
        }

        public bool Contains<TData>()
        {
            return _typeData.Contains<TData>();
        }

        public bool Contains(Type type)
        {
            return _typeData.Contains(type);
        }
        
        public bool HasValue()
        {
            return _typeData.HasValue();
        }
        
        #endregion

    }
}