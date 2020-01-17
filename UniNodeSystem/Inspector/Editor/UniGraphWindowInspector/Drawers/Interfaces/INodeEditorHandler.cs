namespace UniGreenModules.UniNodeSystem.Inspector.Editor.Drawers.Interfaces
{
    using BaseEditor.Interfaces;
    using Runtime.Core;

    public interface INodeEditorHandler
    {
        
        bool Update(INodeEditor editor,UniBaseNode node);
        
    }
}