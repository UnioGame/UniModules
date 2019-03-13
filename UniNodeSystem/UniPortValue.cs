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
            var result = _typeData.Remove<TData>();
            if (result)
            {
                _broadcastContext.Remove<TData>();
            }

            return result;
        }

        public bool Remove(Type type)
        {
            return _typeData.Remove(type);
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


        public TData Get<TData>()
        {
            throw new NotImplementedException();
        }

        public bool Contains<TData>()
        {
            throw new NotImplementedException();
        }

        public bool Contains(Type type)
        {
            throw new NotImplementedException();
        }
    }
}