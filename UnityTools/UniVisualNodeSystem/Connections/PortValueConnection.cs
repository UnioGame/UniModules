using UniModule.UnityTools.Interfaces;

namespace UniModule.UnityTools.UniVisualNodeSystem.Connections
{
    public class PortValueConnection : ContextConnection<IContext>
    {

        public override void UpdateValue<TData>(IContext context, TData value)
        {
            _target.UpdateValue(context,value);
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
