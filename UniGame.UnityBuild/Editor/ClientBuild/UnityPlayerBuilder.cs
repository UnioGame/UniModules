namespace UniGreenModules.UniGame.UnityBuild.Editor.ClientBuild
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Commands.PostBuildCommands;
    using Commands.PreBuildCommands;
    using global::UniCore.Runtime.ProfilerTools;
    using Interfaces;
    using UniCore.EditorTools.Editor.AssetOperations;
    using UniModules.UniGame.UnityBuild.Editor.ClientBuild.BuildConfiguration;
    using UnityEditor;
    using UnityEditor.Build.Reporting;
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
    
            LogBuildStep($"OUTPUT LOCATION : {outputLocation}");

            var report = BuildPipeline.BuildPlayer(scenes, outputLocation,
                buildParameters.BuildTarget, buildOptions);

            LogBuildStep(report.ReportMessage());

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
            //load build command maps
            var commandsMap = AssetEditorTools.
                GetEditorResources<UniBuildCommandsMap>();
            
            var commandsResources = new List<IEditorAssetResource>();
            //filter all valid commands map
            foreach (var mapResource in commandsMap) {

                var commandMap = mapResource.Load<IUniBuildCommandsMap>();
                if(!commandMap.Validate(configuration) ) 
                    continue;

                LogBuildStep($"BUILD: USE COMMAND MAP {commandMap.ItemName}");
                
                var assetResources = commandMap.
                    LoadCommands<TTarget>(x => ValidateCommand(configuration,x));
                
                commandsResources.AddRange(assetResources);
            }

            ExecuteCommands(commandsResources, action);
        }

        public bool ValidateCommand( IUniBuilderConfiguration configuration,IUnityBuildCommand command)
        {
            var asset = command;
            var isValid = asset != null && asset.Info.IsActive;
            if (asset is IUnityBuildCommandValidator validator) {
                isValid = isValid && validator.Validate(configuration);
            }
            return isValid;
        }

        public void ExecuteCommands<TTarget>(
            List<IEditorAssetResource> targetCommands, 
            Action<TTarget> action)
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
                var priority = commandAsset.Info.Priority;
                
                LogBuildStep($"EXECUTE COMMAND {commandName} with priority {priority}");
                
                var startTime = DateTime.Now;
        
                action?.Invoke(commandAsset);

                var endTime       = DateTime.Now;
                var executionTime = endTime - startTime;
                
                LogBuildStep($"EXECUTE COMMAND FINISHED {commandName} DURATION: {executionTime.TotalSeconds}");
            }
        }

        public void LogBuildStep(string message)
        {
            GameLog.Log($"=====BUILD : \n\t{message} =====\n");
        }
        
    }
}
