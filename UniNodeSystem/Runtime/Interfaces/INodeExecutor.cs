namespace UniGreenModules.UniNodeSystem.Runtime.Interfaces
{
    public interface INodeExecutor<in TContext>
    {
        
        void Execute(UniGraphNode node, TContext context);

        void Stop(UniGraphNode node);
        
    }
}