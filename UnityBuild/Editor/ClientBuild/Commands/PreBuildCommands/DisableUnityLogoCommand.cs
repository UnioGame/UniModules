namespace UniGreenModules.UnityBuild.Editor.ClientBuild.Commands.PreBuildCommands {
    using Interfaces;
    using UnityEditor;
    using UnityEngine;

    [CreateAssetMenu(menuName = "UnityBuild/Commands/Disable Unity Logo", fileName = "DisableUnityLogo")]

    public class DisableUnityLogoCommand : UnityPreBuildCommand
    {
        public override void Execute(IArgumentsProvider arguments, IBuildParameters buildParameters) {
            PlayerSettings.SplashScreen.showUnityLogo = false;
        }
    }
}
