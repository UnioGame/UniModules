namespace UniGreenModules.UniNodeSystem.Nodes.Commands
{
    using Runtime.Interfaces;

    public interface IPortPair
    {
        IPortValue InputPort { get; }
        IPortValue OutputPort { get; }
    }
}