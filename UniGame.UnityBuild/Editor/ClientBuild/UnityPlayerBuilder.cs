namespace UniGreenModules.UniGame.UnityBuild.Editor.ClientBuild
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Commands.PostBuildCommands;
    using Commands.PreBuildCommands;
    using Interfaces;
    using UniCore.EditorTools.Editor.AssetOperations;
    using UnityEditor;
    using UnityEditor.Build.Reporting;
    using UnityEngine;
    using Object = UnityEngine.Object;

    public class UnityPlayerBuilder : IUnityPlayerBuilder
    {
        private const string BuildFolder = "Build";

        public BuildReport Build(IUniBuilderConfiguration configuration)
        {

            ExecuteCommands<UnityPreBuildCommand>(configuration,x => x.Execute(configuration));
    
            var result = ExecuteBuild(configuration);
    
            ExecuteCommands<UnityPostBuildCommand>(configuration,x => x.Execute(configuration,result));

            return result;

        }

        private BuildReport ExecuteBuild(IUniBuilderConfiguration configuration)
        {
            var scenes = GetBuildInScenes(configuration);

            var buildParameters = configuration.BuildParameters;
            var outputLocation = GetTargetBuildLocation(configuration.BuildParameters);
            var buildOptions   = buildParameters.BuildOptions;
    
            Debug.Log($"OUTPUT LOCATION : {outputLocation}");
    
            var report = BuildPipeline.BuildPlayer(scenes, outputLocation,
                buildParameters.BuildTarget, buildOptions);

            Debug.Log(report.ReportMessage());

            return report;

        }

        private EditorBuildSettingsScene[] GetBuildInScenes(IUniBuilderConfiguration configuration)
        {
            var parameters = configuration.BuildParameters;
            var scenes = parameters.Scenes.Count > 0 ? parameters.Scenes :
                EditorBuildSettings.scenes;
            return scenes.Where(x => x.enabled).ToArray();
        }

        private string GetTargetBuildLocation(IBuildParameters buildParameters)
        {
            var file   = buildParameters.OutputFile;
            var folder = buildParameters.OutputFolder;
            return $"{folder}/{buildParameters.BuildTarget.ToString()}/{file}";
        }

        public void ExecuteCommands<TTarget>(
            IUniBuilderConfiguration configuration,
            Action<TTarget> action) 
            where  TTarget : Object,IUnityBuildCommand
        {

            var targetCommands = AssetEditorTools.GetEditorResources<TTarget>().
                Where(x => {
                    var asset = x.Load<TTarget>();
                    var isValid = asset != null && asset.Info.IsActive;
                    if (asset is IUnityBuildCommandValidator validator) {
                        isValid = isValid && validator.Validate(configuration);
                    }
                    return isValid;
                }).
                ToList();

            ExecuteCommands(targetCommands, action);

        }

        public void ExecuteCommands<TTarget>(List<EditorAssetResource> targetCommands, Action<TTarget> action)
            where TTarget : Object, IUnityBuildCommand
        {
            var executingCommands = targetCommands.
                OrderByDescending(x => x.Load<TTarget>().Priority).
                ToList();

            foreach (var command in executingCommands) {

                var commandAsset = command.Load<TTarget>();
                if(commandAsset== null)
                    continue;
        
                var commandName    = commandAsset.name;
                var executionOrder = commandAsset.Info.Priority;
        
                Debug.Log($"\n\n=====EXECUTE COMMAND {commandName} with priority {executionOrder}=====");
                var startTime = DateTime.Now;
        
                action?.Invoke(commandAsset);

                var endTime       = DateTime.Now;
                var executionTime = endTime - startTime;
                
                Debug.Log($"=====EXECUTE COMMAND FINISHED {commandName} DURATION:{executionTime.TotalSeconds}=====\n\n");
        
            }
        }

    
    }
}
