namespace Plavalaguna.Joy.Modules.UnityBuild 
{
    using Build;
    using UnityEditor;

    public interface IUnityBuildData {
        
        int Order { get; }

        bool IsActive { get; }

        string Name { get; }

    }
}