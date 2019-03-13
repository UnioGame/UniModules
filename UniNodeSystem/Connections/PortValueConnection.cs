using System.Collections.Generic;
using UniModule.UnityTools.Common;
using UniModule.UnityTools.ProfilerTools;

namespace UniModule.UnityTools.UniVisualNodeSystem.Connections
{
    public class PortValueConnection : 
        ITypeDataContainer,
        IConnector<ITypeDataContainer>
    {
        private readonly List<ITypeDataContainer> _connections;
        private readonly ITypeDataContainer _target;

        public PortValueConnection(ITypeDataContainer target)
        {
            _target = target;
            _connections = new List<ITypeDataContainer>();
        }
        
        public void SetValue<TData>(TData value)
        {
            GameProfiler.BeginSample("Connection_UpdateValue");
            
            _target.SetValue(value);
            
            GameProfiler.EndSample();
        }

        public virtual bool Remove<TData>()
        {

            for (int i = 0; i < _connections.Count; i++)
            {
                var connection = _connections[i];
                if (connection.HasValue<TData>(context))
                {
                    var value = connection.Get<TData>(context);
                    _target.UpdateValue(context,value);
                    return false;
                }
            }
            var result = _target.Remove<TData>();

            return result;
        }

        public void Connect(ITypeDataContainer connection)
        {
            _connections.Add(connection);
        }

        public void Remove(ITypeDataContainer connection)
        {
            _connections.Remove(connection);
        }
    }
}
