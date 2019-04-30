namespace UniGreenModules.UniContextData.Runtime.Interfaces
{
    using UniModule.UnityTools.Interfaces;

    public interface IEntity : IContext
    {
        int Id { get; }
    }
}