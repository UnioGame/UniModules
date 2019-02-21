namespace UniUiSystem
{
    public interface INamedItem
    {
        string ItemName { get; }
        void SetName(string name);
    }
}