using UniModule.UnityTools.Common;
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

        public InputPortConnection(UniGraphNode node, 
            ITypeDataContainer target,
            INodeExecutor<IContext> nodeExecutor) : 
            base(target)
        {
            _node = node;
            _nodeExecutor = nodeExecutor;
        }

        public override bool Remove<TData>()
        {
            
            var result = base.Remove<TData>();
            if (!result || !_node.IsActive) return result;
            
            if (_target.HasValue())
            {
                _nodeExecutor.Stop(_node);     
            }
            return result;
            
        }

        public override void Add<TData>(TData value)
        {
            
            GameProfiler.BeginSample("InputConnection_UpdateValue");
            
            base.Add(value);
            
            if (_node.IsActive == false && _target.HasValue())
            {
                _nodeExecutor.Execute(_node,_target);
            }
            
            GameProfiler.EndSample();
        }
    }
}
