using UniStateMachine;

namespace Modules.UniTools.UniNodeSystem.Editor.Drawers
{
    public interface INodeEditorDrawer
    {
        UniNode DrawNodeBody(UniNode node);
        UniNode DrawNodeHeader(UniNode node);
    }
}