namespace UniGreenModules.UniNodeSystem.Inspector.Editor.Drawers.Interfaces
{
    using BaseEditor.Interfaces;
    using Runtime.Core;

    public interface INodeEditorDrawer
    {
        
        bool Draw(INodeEditor editor,UniBaseNode node);
        
    }
}