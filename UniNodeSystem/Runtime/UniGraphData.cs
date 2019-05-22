using System.Linq;
using UniModule.UnityTools.Interfaces;

namespace UniStateMachine.Nodes
{
    using UniGreenModules.UniCore.Runtime.DataStructure;
    using UniGreenModules.UniCore.Runtime.Interfaces;
    using UniGreenModules.UniCore.Runtime.ObjectPool;
    using UniGreenModules.UniCore.Runtime.ObjectPool.Interfaces;

    public class UniGraphData1 : DoubleKeyDictionary<UniNode, IContext, NodeContextData>, IPoolable
    {

        public IContext Context;
        
        public void Initialize(IContext context)
        {
            Context = context;
        }
        
        public void Release(UniNode key)
        {
            if (!_dictionary.TryGetValue(key, out var nodeContexts))
            {
                return;
            }

            foreach (var data in nodeContexts)
            {
                data.Value.Release();
                data.Value.Despawn();
            }
            nodeContexts.Clear();
            nodeContexts.Despawn();
            _dictionary.Remove(key);

        }

        public void Release(UniNode key, IContext subKey)
        {
            if (!_dictionary.TryGetValue(key, out var nodeContexts))
            {
                return;
            }

            if (!nodeContexts.TryGetValue(subKey, out var data))
            {
                return;
            }

            data.Release();
            data.Despawn();
            nodeContexts.Remove(subKey);
        }

        public void Release()
        {
            var items = _dictionary.Keys.ToList();
            foreach (var item in items)
            {
                Release(item);
            }

            Context = null;
        }

    }
}