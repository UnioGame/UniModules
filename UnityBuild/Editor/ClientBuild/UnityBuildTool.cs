using System;
using System.Linq;
using UniGreenModules.UnityBuild.Editor.ClientBuild;
using UniGreenModules.UnityBuild.Editor.ClientBuild.Commands;
using UniGreenModules.UnityBuild.Editor.ClientBuild.Interfaces;
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
        var buildTargetGroup = GetBuildTargetGroup();
        
        var report      = BuildPlayer(buildTarget,buildTargetGroup, argumentsProvider);

        EditorApplication.Exit(report.summary.result == BuildResult.Succeeded ? 0 : 1);
    }

    public static BuildReport BuildPlayer(BuildTarget buildTarget,BuildTargetGroup buildTargetGroup, ArgumentsProvider argumentsProvider) {

        var buildParameters = ParseBuildArguments(buildTarget,buildTargetGroup, argumentsProvider);

        ExecuteCommands<UnityPreBuildCommand>(x => 
            x.Execute(argumentsProvider, buildParameters));
    
        var result = ExecuteBuild(buildParameters);
    
        ExecuteCommands<UnityPostBuildCommand>(x => 
            x.Execute(argumentsProvider, buildParameters,result));

        return result;

    }

    public static BuildParameters ParseBuildArguments(BuildTarget buildTarget, 
        BuildTargetGroup buildTargetGroup,
        ArgumentsProvider parser)
    {        
        var parameters = new BuildParameters();

        parameters.buildTarget = buildTarget;
        parameters.buildTargetGroup = buildTargetGroup;

        if (parser.GetIntValue(BuildArguments.BuildNumberKey, out var version))
            parameters.buildNumber = version;

        parser.GetStringValue(BuildArguments.BuildOutputFolderKey, out var folder, BuildFolder);
        parameters.outputFolder = folder;

        parser.GetStringValue(BuildArguments.BuildOutputNameKey, out var file, string.Empty);

        parameters.outputFile = file;

        return parameters;
    }

    public static BuildReport ExecuteBuild(BuildParameters buildParameters)
    {
        var scenes = GetBuildInScenes();
    
        var outputLocation = GetTargetBuildLocation(buildParameters);
        var buildOptions   = buildParameters.buildOptions;
    
        Debug.Log($"OUTPUT LOCATION : {outputLocation}");
    
        var report = BuildPipeline.BuildPlayer(scenes, outputLocation,
            buildParameters.buildTarget, buildOptions);

        Debug.Log(report.ReportMessage());

        return report;

    }

    public static BuildTarget GetBuildTarget()
    {
        return EditorUserBuildSettings.activeBuildTarget;
    }

    public static BuildTargetGroup GetBuildTargetGroup()
    {
        return EditorUserBuildSettings.selectedBuildTargetGroup;
    }

    public static EditorBuildSettingsScene[] GetBuildInScenes()
    {
        var scenes = EditorBuildSettings.scenes;
        return scenes.Where(x => x.enabled).ToArray();
    }

    public static string GetTargetBuildLocation(BuildParameters buildParameters)
    {
        var file   = buildParameters.outputFile;
        var folder = buildParameters.outputFolder;
        return $"{folder}/{buildParameters.BuildTarget.ToString()}/{file}";
    }

    public static void ExecuteCommands<TTarget>(Action<TTarget> action) 
        where  TTarget : Object,IUnityBuildCommand
    {

        var targetCommands = AssetEditorTools.GetEditorResources<TTarget>().
            Where(x => {
                var asset = x.Load<TTarget>();
                return asset != null && asset.Info.IsActive;
            }).ToList();
    
        targetCommands.Sort((x,y) => x.Load<TTarget>().
            Info.Order.CompareTo(y.Load<TTarget>().Info.Order));

        foreach (var command in targetCommands) {

            var commandAsset = command.Load<TTarget>();
            if(commandAsset== null)
                continue;
        
            var commandName = commandAsset.name;
            var executionOrder    = commandAsset.Info.Order;
        
            Debug.Log($"=====EXECUTE COMMAND {commandName} with priority {executionOrder}=====");
            var startTime = DateTime.Now;
        
            action(commandAsset);

            var endTime       = DateTime.Now;
            var executionTime = endTime - startTime;
            Debug.Log($"=====EXECUTE COMMAND FINISHED {commandName} TIME:{executionTime.TotalSeconds}=====");
        
        }
    
    }

}
