namespace UniModules.UniGame.BuildCommands.Editor.Addressables
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using UniGreenModules.UniCore.EditorTools.Editor.Utility;
    using UnityEditor;
    using UnityEditor.AddressableAssets;
    using UnityEditor.AddressableAssets.Settings;
    using UnityEngine;

    [Serializable]
    public class ApplyAddressablesTemplatesCommand
    {
        [SerializeField] private string                        filter     = String.Empty;
        [SerializeField] private bool                          useRegExpr = false;
        [SerializeField] private List<AddressableAssetGroup>   localGroups;
        [SerializeField] private AddressableAssetGroupTemplate localTemplate;
        [SerializeField] private AddressableAssetGroupTemplate remoteTemplate;
        
        public void Execute()
        {
            var regExprValue = new Regex(filter,RegexOptions.Compiled|RegexOptions.IgnoreCase);
            
            var settings = AddressableAssetSettingsDefaultObject.Settings;
            if (settings == null) {
                Debug.LogError("Addressable assets settings not found");
                return;
            }

            var lobbyGroups = settings.
                groups.
                Where(g => useRegExpr ? regExprValue.IsMatch(g.Name) : g.Name.StartsWith(filter));
            
            foreach (var group in lobbyGroups)
            {
                var template = localGroups.Contains(group) ? localTemplate : remoteTemplate;
                template.ApplyToAddressableAssetGroup(group);
                group.MarkDirty();
            }
            
            AssetDatabase.SaveAssets();
        }
    }
}