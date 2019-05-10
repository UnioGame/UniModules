namespace Plavalaguna.Joy.Modules.UnityBuild {
    using Build;
    using UnityEditor;
    using UnityEditor.Build.Reporting;

    public interface IUnityPostBuildCommand : IUnityBuildData {
        void Execute(BuildTarget target, IArgumentsProvider arguments, IBuildParameters buildParameters,BuildReport buildReport = null);
    }
}