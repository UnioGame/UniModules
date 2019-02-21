namespace UniUiSystem
{
    public interface INamedItem
    {
        string ItemName { get; }
        void ApplyName(string name);
    }
}