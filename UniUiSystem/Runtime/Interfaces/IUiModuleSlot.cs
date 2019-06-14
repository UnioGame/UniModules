namespace UniGreenModules.UniUiSystem.Runtime.Interfaces
{
    public interface IUiModuleSlot
    {
        string SlotName { get; }

        void Add(IUiModule target);
    }
}