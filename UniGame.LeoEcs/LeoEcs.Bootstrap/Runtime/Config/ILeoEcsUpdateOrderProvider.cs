using UniGame.LeoEcs.Bootstrap.Runtime.Abstract;

namespace UniGame.LeoEcs.Bootstrap.Runtime.Config
{
    public interface ILeoEcsUpdateOrderProvider
    {
        ILeoEcsExecutor Create();
    }
    
    public interface ILeoEcsSystemsPluginProvider
    {
        ISystemsPlugin Create();
    }
}