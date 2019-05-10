namespace Plavalaguna.Joy.Modules.UnityBuild {
    using Build;
    using UnityEditor;
    using UnityEditor.Build.Reporting;

    public abstract class UnityPostBuildCommand : UnityBuildData, IUnityPostBuildCommand {


        public abstract void Execute(BuildTarget target, IArgumentsProvider arguments, IBuildParameters buildParameters,BuildReport buildReport);

    }
}
