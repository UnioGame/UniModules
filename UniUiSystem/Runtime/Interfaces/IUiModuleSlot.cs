namespace UniGreenModules.UniUiSystem.Runtime.Interfaces
{
    using GBG.UI.Runtime.Interfaces;
    using UniNodeSystem.Runtime.Connections;

    public interface IUiModuleSlot : IUiPlacement
    {
        
        string SlotName { get; }

        ITypeDataBrodcaster Value { get; }
        
    }
}