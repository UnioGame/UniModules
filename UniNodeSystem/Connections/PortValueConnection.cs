using System.Collections.Generic;
using UniModule.UnityTools.Common;
using UniModule.UnityTools.ProfilerTools;

namespace UniModule.UnityTools.UniVisualNodeSystem.Connections
{
    public class PortValueConnection : 
        IContextWriter,
        IConnector<ITypeDataContainer>
    {
        protected readonly List<ITypeDataContainer> _connections;
        protected readonly ITypeDataContainer _target;

        public PortValueConnection(ITypeDataContainer target)
        {
            _target = target;
            _connections = new List<ITypeDataContainer>();
        }

        public void Add<TData>(TData value)
        {
            GameProfiler.BeginSample("Connection_UpdateValue");
            
            _target.Add(value);
            
            GameProfiler.EndSample();
        }

        public virtual bool Remove<TData>()
        {

            for (var i = 0; i < _connections.Count; i++)
            {
                var connection = _connections[i];
                if (connection.Contains<TData>())
                {
                    var value = connection.Get<TData>();
                    _target.Add(value);
                    return false;
                }
            }
            
            var result = _target.Remove<TData>();
            return result;
        }

        public IConnector<ITypeDataContainer> Connect(ITypeDataContainer connection)
        {
            _connections.Add(connection);
            return this;
        }

        public void Disconnect(ITypeDataContainer connection)
        {
            _connections.Remove(connection);
        }

    }
}
