using UniStateMachine;
using UniStateMachine.NodeEditor;
using UniNodeSystemEditor;

namespace Modules.UniTools.UniNodeSystem.Drawers
{
    using UniGreenModules.UniNodeSystem.Runtime.Runtime;

    public interface INodeEditorDrawer
    {
        
        bool Draw(INodeEditor editor,UniBaseNode node);
        
    }
}