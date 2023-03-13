namespace UniGame.LeoEcs.Debug.Editor
{
    public interface IEntityEditorView
    {
        public int Id { get; }
        public string Name { get; }
        public void Show();
    }
}