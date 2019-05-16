namespace UniGreenModules.UnityBuild.Editor.ClientBuild
{
    using UnityEditor.Build.Reporting;

    public interface IUnityPlayerBuilder
    {
        BuildReport Build(IUniBuilderConfiguration configuration);
    }
}