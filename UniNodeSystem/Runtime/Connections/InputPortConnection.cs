using UniModule.UnityTools.Interfaces;
using UniStateMachine;
using UniStateMachine.Nodes;
using UnityEngine;

namespace UniModule.UnityTools.UniVisualNodeSystem.Connections
{
    using UniGreenModules.UniCore.Runtime.Interfaces;
    using UniGreenModules.UniCore.Runtime.ProfilerTools;

    public class InputPortConnection : PortValueConnection
    {
        private readonly INodeExecutor<IContext> _nodeExecutor;
        private readonly UniGraphNode _node;

        public InputPortConnection(UniGraphNode node, 
            ITypeData target,
            INodeExecutor<IContext> nodeExecutor) : 
            base(target)
        {
            _node = node;
            _nodeExecutor = nodeExecutor;
        }

        public override bool Remove<TData>()
        {
            
            var result = base.Remove<TData>();
            
            //is node should be stoped
            if (!result || !_node.IsActive) return result;
            
            if (target.HasValue() == false)
            {
                _nodeExecutor.Stop(_node);     
            }
            
            return true;
            
        }

        public override void CleanUp()
        {
            base.CleanUp();
            _nodeExecutor.Stop(_node);   
        }

        public override void Add<TData>(TData value)
        {
            
            GameProfiler.BeginSample("InputConnection_UpdateValue");
            
            base.Add(value);

            if (_node.IsActive == false)
            {
                var context = target.Get<IContext>();
                if(context != null)
                    _nodeExecutor.Execute(_node,context);
            }
            
            GameProfiler.EndSample();
        }
    }
}
