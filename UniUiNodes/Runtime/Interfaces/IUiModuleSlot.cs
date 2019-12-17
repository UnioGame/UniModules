namespace UniGreenModules.UniUiNodes.Runtime.Interfaces
{
    using UniNodeSystem.Runtime.Connections;
    using UniNodeSystem.Runtime.Interfaces;
    using UniUiSystem.Runtime.Interfaces;

    public interface IUiModuleSlot : IUiPlacement
    {
        
        string SlotName { get; }

        ITypeDataBrodcaster Value { get; }
        
    }
}