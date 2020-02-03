namespace UniGreenModules.UniContextData.Runtime.Interfaces
{
    using UniCore.Runtime.Interfaces;

    public interface IEntity : IContext
    {
        int Id { get; }
    }
}