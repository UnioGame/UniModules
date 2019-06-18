namespace UniGreenModules.UniUiNodes.Runtime.Interfaces
{
    using UniNodeSystem.Runtime.Connections;
    using UniUiSystem.Runtime.Interfaces;

    public interface IUiModuleSlot : IUiPlacement
    {
        
        string SlotName { get; }

        ITypeDataBrodcaster Value { get; }
        
    }
}