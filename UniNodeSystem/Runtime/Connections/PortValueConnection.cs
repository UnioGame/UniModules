
namespace UniGreenModules.UniNodeSystem.Runtime.Connections
{
    using System.Collections.Generic;
    using Interfaces;
    using UniCore.Runtime.Interfaces;
    using UniCore.Runtime.ProfilerTools;

    public class PortValueConnection : 
        IContextWriter,
        IConnector<ITypeData>
    {
        protected readonly List<ITypeData> connections;
        protected readonly ITypeData target;

        public PortValueConnection(ITypeData target)
        {
            this.target = target;
            connections = new List<ITypeData>();
        }

        public virtual void Add<TData>(TData value)
        {
            GameProfiler.BeginSample("Connection_UpdateValue");
            
            target.Add(value);
            
            GameProfiler.EndSample();
        }

        public virtual void CleanUp()
        {
            target.CleanUp();
        }
        
        public virtual bool Remove<TData>()
        {

            for (var i = 0; i < connections.Count; i++)
            {
                var connection = connections[i];
                if (connection.Contains<TData>())
                {
                    var value = connection.Get<TData>();
                    target.Add(value);
                    return false;
                }
            }
            
            var result = target.Remove<TData>();
            return result;
        }

        public IConnector<ITypeData> Connect(ITypeData connection)
        {
            connections.Add(connection);
            return this;
        }

        public void Disconnect(ITypeData connection)
        {
            connections.Remove(connection);
        }

    }
}
