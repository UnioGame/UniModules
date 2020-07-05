using UnityEngine;

namespace UniModules.UniGame.SceneEditorOnlyAssets.Editor
{
    using global::UniGame.SceneEditorOnlyAssets.Editor;
    using UnityEditor.Build;
    using UnityEditor.Build.Reporting;

    /// <summary>
    /// Mark Scene Editor Asset as Disabled during player building process
    /// </summary>
    public class SceneEditorAssetsBuildHandler : IPreprocessBuildWithReport,IPostprocessBuildWithReport
    {
        public int callbackOrder => 0;
        
        public void OnPostprocessBuild(BuildReport report)
        {
            SceneEditorAssetsProcessor.SetActive(true);
        }

        public void OnPreprocessBuild(BuildReport report)
        {
            SceneEditorAssetsProcessor.CloseAllCommands();
        }
    }
}
