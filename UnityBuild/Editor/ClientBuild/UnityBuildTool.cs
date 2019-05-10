using System;
using System.Collections.Generic;
using System.Linq;
using Build;
using Plavalaguna.Joy.Modules.UnityBuild;
using UniGreenModules.UniCore.EditorTools.AssetOperations;
using UniModule.UnityTools.EditorTools;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;
using Object = UnityEngine.Object;

public static class UnityBuildTool
{

    public const string BuildFolder = "Build";
    
    /// <summary>
    /// Console build call. Close editor after end of build process
    /// </summary>
    public static void BuildUnityPlayer()
    {
        var argumentsProvider = new ArgumentsProvider(Environment.GetCommandLineArgs());
        Debug.Log(argumentsProvider);

        var buildTarget = GetBuildTarget();
        var report = BuildPlayer(buildTarget, argumentsProvider);

        EditorApplication.Exit(report.summary.result == BuildResult.Succeeded ? 0 : 1);
    }
    
    public static BuildReport BuildPlayer(BuildTarget buildTarget, ArgumentsProvider argumentsProvider) {

        var buildParameters = ParseBuildArguments(buildTarget, argumentsProvider);

        ExecuteCommands<IUnityPreBuildCommand>(x => 
            x.Execute(buildTarget, argumentsProvider, buildParameters));
        
        var result = ExecuteBuild(buildParameters);
        
        ExecuteCommands<IUnityPostBuildCommand>(x => 
            x.Execute(buildTarget, argumentsProvider, buildParameters,result));

        return result;

    }

    public static BuildParameters ParseBuildArguments(BuildTarget buildTarget, 
        ArgumentsProvider parser)
    {        
        var parameters = new BuildParameters();

        parameters.buildTarget = buildTarget;

        if (parser.GetIntValue(BuildArguments.BuildNumberKey, out var version))
            parameters.buildNumber = version;

        parser.GetStringValue(BuildArguments.BuildOutputFolderKey, out var folder, BuildFolder);
        parameters.outputFolder = folder;

        parser.GetStringValue(BuildArguments.BuildOutputNameKey, out var file, string.Empty);

        parameters.outputFile = file;
        parameters.buildOptions = GetBuildOptions(parser);
        
        return parameters;
    }

    public static BuildReport ExecuteBuild(BuildParameters buildParameters)
    {
        var scenes = GetBuildInScenes();
        
        var outputLocation = GetTargetBuildLocation(buildParameters);
        var buildOptions = buildParameters.buildOptions;
        
        Debug.Log($"OUTPUT LOCATION : {outputLocation}");
        
        var report = BuildPipeline.BuildPlayer(scenes, outputLocation,
            buildParameters.buildTarget, buildOptions);

        Debug.Log(report.ReportMessage());

        return report;

    }

    public static BuildOptions GetBuildOptions(ArgumentsProvider parameters)
    {
        var options = BuildOptions.None;
        
        if (parameters.Contains(BuildArguments.DeveloperBuildKey))
        {
            options |= BuildOptions.Development;
        }
        if (parameters.Contains(BuildArguments.ScriptDebugBuildKey))
        {
            options |= BuildOptions.AllowDebugging;
        }
        
        return options;
    }


    public static BuildTarget GetBuildTarget()
    {
        return EditorUserBuildSettings.activeBuildTarget;
    }

    public static EditorBuildSettingsScene[] GetBuildInScenes()
    {
        var scenes = EditorBuildSettings.scenes;
        return scenes.Where(x => x.enabled).ToArray();
    }

    public static string GetTargetBuildLocation(BuildParameters buildParameters)
    {
        var file = buildParameters.outputFile;
        var folder = buildParameters.outputFolder;
        return $"{folder}/{buildParameters.BuildTarget.ToString()}/{file}";
    }
    
    public static void ExecuteCommands<TTarget>(Action<TTarget> action) 
        where  TTarget :class, IUnityBuildData {

        var targetCommands = AssetEditorTools.GetEditorResources<UnityBuildData>().
            Where(x => {
                var asset = x.Load<TTarget>();
                return asset != null && asset.IsActive;
            }).ToList();
        
        targetCommands.Sort((x,y) => x.Load<TTarget>().Priority.CompareTo(y.Load<TTarget>().Priority));

        foreach (var command in targetCommands) {

            var commandAsset = command.Load<TTarget>();
            if(commandAsset== null)
                continue;
            
            var commandName = commandAsset.Name;
            var priority = commandAsset.Priority;
            
            Debug.Log($"=====EXECUTE COMMAND {commandName} with priority {priority}=====");
            var startTime = DateTime.Now;
            
            action(commandAsset);

            var endTime       = DateTime.Now;
            var executionTime = endTime - startTime;
            Debug.Log($"=====EXECUTE COMMAND FINISHED {commandName} TIME:{executionTime.TotalSeconds}=====");
            
        }
        
    }
    
}