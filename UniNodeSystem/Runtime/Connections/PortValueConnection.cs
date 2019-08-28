namespace UniGreenModules.UniNodeSystem.Runtime.Connections
{
    using System.Collections.Generic;
    using Interfaces;
    using Runtime;
    using UniCore.Runtime.Interfaces;
    using UniCore.Runtime.ProfilerTools;

    public class PortValueConnection : IPortConnection
    {
        private List<ITypeData>  connections          = new List<ITypeData>();
        private ITypeData        target;
        private INodePort        targetPort;

        public void Initialize(ITypeData targetContainer)
        {
            this.target      = targetContainer;
            this.connections = new List<ITypeData>();
        }

        public void Release()
        {
            connections.Clear();

            targetPort = null;
            target     = null;
        }

        public virtual void Publish<TData>(TData value)
        {
            GameProfiler.BeginSample("Connection_UpdateValue");

            target.Publish(value);

            GameProfiler.EndSample();
        }

        public virtual void CleanUp()
        {
            target?.CleanUp();
        }

        public virtual bool Remove<TData>()
        {
            for (var i = 0; i < connections.Count; i++) {
                var connection = connections[i];
                if (connection.Contains<TData>()) {
                    var value = connection.Get<TData>();
                    target.Publish(value);
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