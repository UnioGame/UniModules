namespace UniGreenModules.UniNodeSystem.Runtime.Interfaces
{
    public interface INodeExecutor<in TContext>
    {
        
        void Execute(IUniNode node, TContext context);

        void Stop(IUniNode node);
        
    }
}