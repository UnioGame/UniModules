using UnityEngine;

namespace UniModules.UniGame.UnityBuild.Editor.ClientBuild.BuildConfiguration
{
    using System;
    using System.Collections.Generic;
    using UniGreenModules.UniCore.EditorTools.Editor.AssetOperations;
    using UniGreenModules.UniCore.Runtime.Extension;
    using UniGreenModules.UniCore.Runtime.ObjectPool.Runtime;
    using UniGreenModules.UniCore.Runtime.ObjectPool.Runtime.Extensions;
    using UniGreenModules.UniGame.UnityBuild.Editor.ClientBuild.Commands.PostBuildCommands;
    using UniGreenModules.UniGame.UnityBuild.Editor.ClientBuild.Commands.PreBuildCommands;
    using UniGreenModules.UniGame.UnityBuild.Editor.ClientBuild.Interfaces;

    [CreateAssetMenu(menuName = "UnityBuild/UniBuildConfiguration", fileName = nameof(UniBuildCommandsMap))]
    public class UniBuildCommandsMap : ScriptableObject, IUniBuildCommandsMap
    {
        
        public UniBuildConfigurationData BuildData = new UniBuildConfigurationData();
        
        public List<UnityPreBuildCommand> PreBuildCommands = new List<UnityPreBuildCommand>();
        
        public List<UnityPostBuildCommand> PostBuildCommands = new List<UnityPostBuildCommand>();

        public string ItemName => name;
        
        public List<IEditorAssetResource> LoadCommands<T>(Func<T,bool> filter = null)
            where T : IUnityBuildCommand 
        {
            var result = new List<IEditorAssetResource>();
            
            var commandsBuffer = ClassPool.Spawn<List<UnityBuildCommand>>();
            commandsBuffer.AddRange(PreBuildCommands);
            commandsBuffer.AddRange(PostBuildCommands);
            
            foreach (var command in commandsBuffer) {
                if (command is T targetCommand) {
                    
                    if(filter!=null && !filter(targetCommand))
                        continue;
                    
                    result.Add(new EditorAssetResource().Initialize(command));
                }
            }

            commandsBuffer.Despawn();

            return result;
        }

        public bool Validate(IUniBuilderConfiguration config)
        {
            var buildParameters = config.BuildParameters;

            if (!BuildData.BuildTargets.Contains(buildParameters.BuildTarget))
                return false;

            if (!BuildData.BuildTargetGroups.Contains(buildParameters.BuildTargetGroup))
                return false;

            var isUnderCloud = false;
            
#if UNITY_CLOUD_BUILD
            isUnderCloud = true;
#endif
            
            if (isUnderCloud != BuildData.CloudBuild)
                return false;
            
            return ValidatePlatform(config);
        }

        protected virtual bool ValidatePlatform(IUniBuilderConfiguration config)
        {
            return true;
        }

    }
}
