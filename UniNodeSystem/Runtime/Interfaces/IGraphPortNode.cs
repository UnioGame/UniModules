namespace UniGreenModules.UniNodeSystem.Runtime.Interfaces
{
    using Runtime;

    public interface IGraphPortNode : INode
    {
        PortIO Direction { get; }
        
        UniPortValue PortValue { get; }
    }
}