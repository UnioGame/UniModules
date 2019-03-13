using UniModule.UnityTools.Interfaces;
using UniModule.UnityTools.ProfilerTools;
using UniStateMachine;
using UniStateMachine.Nodes;
using UnityEngine;

namespace UniModule.UnityTools.UniVisualNodeSystem.Connections
{
    public class InputPortConnection : PortValueConnection
    {
        private readonly INodeExecutor<IContext> _nodeExecutor;
        private readonly UniGraphNode _node;

        public InputPortConnection(INodeExecutor<IContext> nodeExecutor,UniGraphNode node, 
            IContextData<IContext> target) : 
            base(target)
        {
            _nodeExecutor = nodeExecutor;
            _node = node;
        }

        public override bool RemoveContext(IContext context)
        {
            GameProfiler.BeginSample("InputConnection_RemoveContext");
            
            var result = base.RemoveContext(context);
            if (result && _node.IsActive)
            {
                _nodeExecutor.Stop(_node,context);
            }

            GameProfiler.EndSample();
            
            return result;
        }

        public override void UpdateValue<TData>(IContext context, TData value)
        {
            GameProfiler.BeginSample("InputConnection_UpdateValue");
            
            var isContextExists = _target.HasContext(context);
            base.UpdateValue(context, value);
            if (isContextExists == false && _target.HasContext(context))
            {
                _nodeExecutor.Execute(_node,context);
            }
            
            GameProfiler.EndSample();
        }
    }
}
