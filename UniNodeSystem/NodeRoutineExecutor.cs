namespace Modules.UniTools.UniNodeSystem
{
    using UniModule.UnityTools.Interfaces;
    using UniModule.UnityTools.UniRoutine;
    using UniModule.UnityTools.UniStateMachine;
    using UniStateMachine;
    using UniStateMachine.Nodes;
    
    public class NodeRoutineExecutor : INodeExecutor<IContext>
    {   
        public void Execute(UniGraphNode node, IContext context)
        {
            if (node.IsActive)
                return;

            StateLogger.LogState($"GRAPH NODE {node.name} : STARTED", node);

            var inputValue = node.Input;
            inputValue.Add(context);

            var awaiter = node.Execute(context);
            var disposable = awaiter.RunWithSubRoutines(node.RoutineType);

            //cleanup actions
            var lifeTime = node.LifeTime;
            lifeTime.AddDispose(disposable);
        }

        public void Stop(UniGraphNode node, IContext context)
        {
            //node already stoped
            if (!node.IsActive)
                return;

            StateLogger.LogState($"GRAPH NODE {node.name} : STOPED", node);

            node.Exit();
        }
    }
}
