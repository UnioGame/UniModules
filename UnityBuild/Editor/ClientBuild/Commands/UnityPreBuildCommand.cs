namespace Plavalaguna.Joy.Modules.UnityBuild {
    using Build;
    using UnityEditor;

    public abstract class UnityPreBuildCommand : UnityBuildData, IUnityPreBuildCommand {

        public abstract void Execute(BuildTarget target, IArgumentsProvider arguments, IBuildParameters buildParameters);

    }
}
