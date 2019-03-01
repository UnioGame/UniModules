namespace UniUiSystem
{
    public interface INamedItem
    {
        string ItemName { get; }
        
    }

    public interface INameWriter : INamedItem
    {
        void ApplyName(string name);
    }
}