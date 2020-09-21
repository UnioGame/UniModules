namespace UniModules.UniGame.BuildCommands.Editor.Addressables
{
    using System;
    using UniBuild.Editor.ClientBuild.Commands.PreBuildCommands;
    using UniBuild.Editor.ClientBuild.Interfaces;
    using UnityEngine;

    [Serializable]
    [CreateAssetMenu(menuName = "UniGame/UniBuild/Addressables/ApplyGroupsTemplateCommand",fileName = nameof(ApplyGroupsTemplateCommand))]
    public class ApplyGroupsTemplateCommand : UnityPreBuildCommand
    {
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.InlineProperty]
        [Sirenix.OdinInspector.HideLabel]
#endif
        [SerializeField]
        private ApplyAddressablesTemplatesCommand command = new ApplyAddressablesTemplatesCommand();

        public override void Execute(IUniBuilderConfiguration buildParameters)
        {
            Execute();
        }

#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.Button]
#endif
        public void Execute()
        {
            command.Execute();
        }
        
    }
}
