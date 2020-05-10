using UnityEngine;

namespace UniModules.UniGame.UnityBuild.Editor.ClientBuild.BuildConfiguration
{
    using System;
    using System.Collections.Generic;
    using UniGreenModules.UniCore.EditorTools.Editor.AssetOperations;
    using UniGreenModules.UniCore.Runtime.ObjectPool.Runtime;
    using UniGreenModules.UniCore.Runtime.ObjectPool.Runtime.Extensions;
    using UniGreenModules.UniGame.UnityBuild.Editor.ClientBuild.Commands;
    using UniGreenModules.UniGame.UnityBuild.Editor.ClientBuild.Commands.PostBuildCommands;
    using UniGreenModules.UniGame.UnityBuild.Editor.ClientBuild.Commands.PreBuildCommands;
    using UniGreenModules.UniGame.UnityBuild.Editor.ClientBuild.Interfaces;

    [CreateAssetMenu(menuName = "UnityBuild/UniBuildConfiguration", fileName = nameof(UniBuildCommandsMap))]
    public class UniBuildCommandsMap : ScriptableObject, IUniBuildCommandsMap
    {
#if  ODIN_INSPECTOR
        [Sirenix.OdinInspector.InlineProperty()]
#endif
        public UniBuildConfigurationData BuildData = new UniBuildConfigurationData();

        [Space]
#if  ODIN_INSPECTOR
        [Sirenix.OdinInspector.InlineEditor()]
#endif
        public List<UnityPreBuildCommand> PreBuildCommands = new List<UnityPreBuildCommand>();
        
#if  ODIN_INSPECTOR
        [Sirenix.OdinInspector.InlineEditor()]
#endif
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

            if (BuildData.BuildTarget != buildParameters.BuildTarget)
                return false;

            if (BuildData.BuildTargetGroup!=buildParameters.BuildTargetGroup)
                return false;

            var isUnderCloud = false;
            
#if UNITY_CLOUD_BUILD
            isUnderCloud = true;
#endif
            
            if (isUnderCloud != BuildData.CloudBuild)
                return false;
            
            return ValidatePlatform(config);
        }

#if  ODIN_INSPECTOR
        [Sirenix.OdinInspector.Button("Execute")]
#endif
        public void ExecuteBuild()
        {
            UnityEditorBuildCommands.ExecuteBuild(BuildData.ArtifactName,BuildData.BuildTarget,BuildData.BuildTargetGroup);
        }
        
        protected virtual bool ValidatePlatform(IUniBuilderConfiguration config)
        {
            return true;
        }

    }
}
