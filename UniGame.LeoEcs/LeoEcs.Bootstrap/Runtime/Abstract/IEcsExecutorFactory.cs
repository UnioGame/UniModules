namespace UniGame.LeoEcs.Bootstrap.Runtime.Abstract
{
    public interface IEcsExecutorFactory
    {
        ILeoEcsExecutor Create(string updateId);
    }
}