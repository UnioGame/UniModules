namespace UniGreenModules.UniGame.UnityBuild.Editor.ClientBuild.Interfaces 
{
    public interface IUnityBuildCommandInfo {
        
        int Priority { get; }

        bool IsActive { get; }

        string Name { get; }
    }
}