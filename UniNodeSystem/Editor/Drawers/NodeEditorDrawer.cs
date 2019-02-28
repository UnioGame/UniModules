using UniStateMachine;

namespace Modules.UniTools.UniNodeSystem.Editor.Drawers
{
    public class NodeEditorDrawer : INodeEditorDrawer
    {

        public virtual UniNode DrawNodeBody(UniNode node)
        {
            return node;
        }

        public virtual UniNode DrawNodeHeader(UniNode node)
        {
            return node;
        }
    
    }
}
