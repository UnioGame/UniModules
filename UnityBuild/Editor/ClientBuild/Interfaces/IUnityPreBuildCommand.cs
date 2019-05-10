namespace Plavalaguna.Joy.Modules.UnityBuild {
    using Build;
    using UnityEditor;

    public interface IUnityPreBuildCommand : IUnityBuildData{
        void Execute(BuildTarget target, IArgumentsProvider arguments, IBuildParameters buildParameters);
    }
}