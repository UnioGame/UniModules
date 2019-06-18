namespace UniGreenModules.UniNodeSystem.Runtime.Interfaces
{
    using Runtime;

    public interface IGraphPortNode : IUniNode
    {
        PortIO Direction { get; }
        
        IPortValue PortValue { get; }

        bool Visible { get; }

    }
}