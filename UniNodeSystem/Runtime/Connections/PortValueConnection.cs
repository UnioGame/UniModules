
namespace UniModule.UnityTools.UniVisualNodeSystem.Connections
{
    using System.Collections.Generic;
    using UniGreenModules.UniCore.Runtime.Interfaces;
    using UniGreenModules.UniCore.Runtime.ProfilerTools;

    public class PortValueConnection : 
        IContextWriter,
        IConnector<ITypeData>
    {
        protected readonly List<ITypeData> _connections;
        protected readonly ITypeData _target;

        public PortValueConnection(ITypeData target)
        {
            _target = target;
            _connections = new List<ITypeData>();
        }

        public virtual void Add<TData>(TData value)
        {
            GameProfiler.BeginSample("Connection_UpdateValue");
            
            _target.Add(value);
            
            GameProfiler.EndSample();
        }

        public virtual void RemoveAll()
        {
            _target.RemoveAll();
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

        public IConnector<ITypeData> Connect(ITypeData connection)
        {
            _connections.Add(connection);
            return this;
        }

        public void Disconnect(ITypeData connection)
        {
            _connections.Remove(connection);
        }

    }
}
