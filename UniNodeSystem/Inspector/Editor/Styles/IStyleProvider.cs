using Modules.UniTools.UniNodeSystem.Editor.BaseEditor;
using UniNodeSystem;
using UnityTools.Interfaces;

namespace Modules.UniTools.UniNodeSystemEditor.Editor.Styles
{
    public interface IStyleProvider : ISelector<NodePort,NodeGuiLayoutStyle>
    {
        
        NodeGuiLayoutStyle Select(NodePort port);
        
    }
}