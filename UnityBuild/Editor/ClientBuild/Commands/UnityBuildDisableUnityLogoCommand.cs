using Build;
using UnityEditor;

namespace Plavalaguna.Joy.Modules.UnityBuild.Editor.ClientBuild.Commands {
    using UnityEngine;

    [CreateAssetMenu(menuName = "UnityBuild/Commands/Disable Unity Logo", fileName = "DisableUnityLogo")]

    public class UnityBuildDisableUnityLogoCommand : UnityPreBuildCommand
    {
        public override void Execute(BuildTarget target, IArgumentsProvider arguments, IBuildParameters buildParameters) {
            PlayerSettings.SplashScreen.showUnityLogo = false;
        }
    }
}
