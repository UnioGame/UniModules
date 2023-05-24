

namespace UniGame.UnityFigma.Editor.FlowGenerator
{
    using UniGame.UiSystem.UI.Editor.UiEdito;
    using UnityEngine;
    using UnityFigmaBridge.Editor.Settings;
    
    [CreateAssetMenu(menuName = "UniGame/Figma/Figma Post Build View Commands",fileName = "Post Build View Commands")]
    public class ViewPostBuildFigmaFlowCommands : PostBuildFigmaFlowCommandAsset
    {
        public override void Execute()
        {
            ViewAssembler.RefreshUiSettings();
        }
    }
}
