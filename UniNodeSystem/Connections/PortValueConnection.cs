using UniModule.UnityTools.Interfaces;
using UniModule.UnityTools.ProfilerTools;

namespace UniModule.UnityTools.UniVisualNodeSystem.Connections
{
    public class PortValueConnection : ContextConnection<IContext>
    {

        public PortValueConnection(IContextData<IContext> target) : base(target){}
        
        public override void UpdateValue<TData>(IContext context, TData value)
        {
            GameProfiler.BeginSample("Connection_UpdateValue");
            
            _target.UpdateValue(context,value);
            
            GameProfiler.EndSample();
        }

        public override bool RemoveContext(IContext context)
        {
            
            for (int i = 0; i < _connections.Count; i++)
            {
                var connection = _connections[i];
                if (connection.HasContext(context))
                {
                    return false;
                }
            }
            
            return _target.RemoveContext(context);
            
        }

        public override bool Remove<TData>(IContext context)
        {
            var result = _target.Remove<TData>(context);
            
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

            return result;
        }

    }
}
