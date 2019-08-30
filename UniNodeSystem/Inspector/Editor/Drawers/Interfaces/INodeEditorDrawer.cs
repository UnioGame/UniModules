namespace UniGreenModules.UniNodeSystem.Inspector.Editor.Drawers
{
    using BaseEditor.Interfaces;
    using Runtime.Runtime;

    public interface INodeEditorDrawer
    {
        
        bool Draw(INodeEditor editor,UniBaseNode node);
        
    }
}