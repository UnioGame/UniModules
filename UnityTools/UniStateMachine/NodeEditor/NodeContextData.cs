using Assets.Tools.UnityTools.Interfaces;
using Assets.Tools.UnityTools.ObjectPool.Scripts;
using Assets.Tools.UnityTools.UniRoutine;

namespace UniStateMachine.Nodes
{
    public class NodeContextData : IPoolable
    {
        public UniNode Node;

        public IDisposableItem DisposableItem;

        public IContext Context;

        public void Activate(UniNode node, IContext nodeContext)
        {
            Release();
            Node = node;
            Context = nodeContext;

            node.Input.Value.AddValue(nodeContext,nodeContext);
            var routine = Node.Execute(Context);
            var routineDisposable = routine.RunWithSubRoutines(Node.RoutineType);
            DisposableItem = routineDisposable;

        }
		
        public void Release()
        {
            Node?.Input.Value.RemoveContext(Context);
            Node?.Exit(Context);
            Node = null;
            DisposableItem?.Dispose();
            DisposableItem = null;
            Context = null;
        }
    }
}