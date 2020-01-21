namespace UniGreenModules.UniNodeSystem.Runtime.Interfaces
{
    public interface IUniInOutNode
    {
        IPortValue Input { get; }
        IPortValue Output { get; }
    }
}