namespace UniGame.UnityBuild.Editor.ClientBuild.Commands.PreBuildCommands.AddressablesCommands
{
    using UniGreenModules.UniGame.UnityBuild.Editor.ClientBuild.Commands.PreBuildCommands;
    using UniGreenModules.UniGame.UnityBuild.Editor.ClientBuild.Interfaces;
    using UnityEditor.AddressableAssets.Build;
    using UnityEditor.AddressableAssets.Settings;
    using UnityEditor.Build.Pipeline.Utilities;
    using UnityEngine;

    public enum CleanType
    {
        CleanAll,
        CleanContentBuilders,
        CleanBuildPipelineCache,
    }
    
    [CreateAssetMenu(menuName = "UnityBuild/PreBuildCommands/Addressables Cleanup", fileName = nameof(AddressablesCleanUpCommand))]
    public class AddressablesCleanUpCommand : UnityPreBuildCommand
    {
                
        public const string AddressablesCachePath = "./Library/com.unity.addressables/StreamingAssetsCopy";
        public const string StreamingAddressablesPath = "./StreamingAssets/aa";
        

        [Tooltip("Clean Addressables Library cache")]
        public bool CleanUpLibraryCache = true;

        public CleanType CleanType = CleanType.CleanAll;
        
        public override void Execute(IUniBuilderConfiguration buildParameters)
        {
            RemoveLibraryCache();
            
            switch (CleanType) {
                case CleanType.CleanAll:
                    CleanAll();
                    break;
                case CleanType.CleanContentBuilders:
                    OnCleanAddressables(null);
                    break;
                case CleanType.CleanBuildPipelineCache:
                    OnCleanSBP();
                    break;
            }
        }

        public void RemoveLibraryCache()
        {
            if (CleanUpLibraryCache == false) return;
            
        }
        
        public void CleanAll()
        {
            OnCleanAddressables(null);
            OnCleanSBP();
        }

        public void OnCleanAddressables(object builder)
        {
            AddressableAssetSettings.CleanPlayerContent(builder as IDataBuilder);
        }

        public void OnCleanSBP()
        {
            BuildCache.PurgeCache(true);
        }
        
    }
}
