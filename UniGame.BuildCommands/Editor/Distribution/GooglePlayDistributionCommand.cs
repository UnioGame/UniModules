using System;
using System.IO;
using ConsoleGPlayAPITool;
using UniModules.UniGame.Core.EditorTools.Editor.Tools;
using UniModules.UniGame.UniBuild.Editor.ClientBuild.Commands.PreBuildCommands;
using UniModules.UniGame.UniBuild.Editor.ClientBuild.Interfaces;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace UniModules.UniGame.BuildCommands.Editor.Distribution
{
    [Serializable]
    public class GooglePlayDistributionCommand : UnitySerializablePostBuildCommand
    {
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.InlineProperty]
        [Sirenix.OdinInspector.HideLabel]
#endif
        [SerializeField]
        public AndroidDistributionSettings settings = new AndroidDistributionSettings();

#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.InfoBox("Use Build path to Artifact location")]
#endif
        public bool useBuildPath;
    
        public override void Execute(IUniBuilderConfiguration configuration, BuildReport buildReport = null)
        {
            settings.Validate();
        
            var buildParameters    = configuration.BuildParameters;
            var outputArtifactPath = buildParameters.OutputFolder.CombinePath(buildParameters.OutputFile);
        
            Debug.Log($"{nameof(GooglePlayDistributionCommand)} : Upload {outputArtifactPath}");

            if (!File.Exists(outputArtifactPath))
            {
                Debug.LogError($"Artifact not found {outputArtifactPath}");
                return;
            }
        
            settings.artifactPath = outputArtifactPath;
            settings.packageName  = PlayerSettings.applicationIdentifier;
            settings.releaseName  = PlayerSettings.bundleVersion;

            var androidPublisher = new PlayStorePublisher();
            androidPublisher.Publish(settings);
        
            Publish(settings);
        }
    
    
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.Button]
#endif
        public void Execute()
        {
            settings.Validate();
            Publish(settings);
        }
    
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.Button]
#endif
        public void Reset()
        {
            settings = new AndroidDistributionSettings();
            settings.Validate();
        }
    
        public void Publish(IAndroidDistributionSettings uploadSettings)
        {
            var androidPublisher = new PlayStorePublisher();
            androidPublisher.Publish(uploadSettings);
        }
    
    }
}
